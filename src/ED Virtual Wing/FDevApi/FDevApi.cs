using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace ED_Virtual_Wing.FDevApi
{
    public class FDevApi : IDisposable
    {
        private const string AuthTokenUrl = "https://auth.frontierstore.net/token";
        private const string AuthUserInfoUrl = "https://auth.frontierstore.net/me";
        private const string ProfileUrl = "https://companion.orerve.net/profile";
        private static HttpClient HttpClient { get; set; } = new();
        private string ClientId { get; }
        private string RedirectUri { get; }
        private IConfiguration Configuration { get; }
        private bool Disposed { get; set; }

        public FDevApi(IConfiguration configuration)
        {
            Configuration = configuration;
            ClientId = configuration["EDVW:FDevClientId"];
            RedirectUri = Configuration["EDVW:FDevAuthReturnUrl"];
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                Disposed = true;
                HttpClient.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public async Task<FDevAuthenticationResult?> AuthenticateUser(string userCode, FDevApiAuthCode fdevApiAuthCode)
        {
            // If this request fails with an error 500, then we probably have a compatibility issue.
            // It was tested and worked with IdentityModel 5.2
            TokenResponse tokenResponse = await HttpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = AuthTokenUrl,
                ClientId = ClientId,
                Code = userCode,
                RedirectUri = RedirectUri,
                CodeVerifier = fdevApiAuthCode.Code,
                GrantType = "authorization_code",
            });
            if (!tokenResponse.IsError)
            {
                UserInfoResponse userInfoResponse = await HttpClient.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = AuthUserInfoUrl,
                    Token = tokenResponse.AccessToken,
                });
                if (!userInfoResponse.IsError)
                {
                    long customerId = Convert.ToInt64(userInfoResponse.TryGet("customer_id") ?? "0");
                    return new FDevAuthenticationResult(tokenResponse, userInfoResponse, customerId);
                }
            }
            return null;
        }

        public string CreateAuthorizeUrl(ApplicationDbContext applicationDbContext)
        {
            string state = CryptoRandom.CreateUniqueId(32);
            string codeVerifier = CryptoRandom.CreateUniqueId(32);
            using SHA256 sha256 = SHA256.Create();
            byte[] challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            string codeChallenge = Base64Url.Encode(challengeBytes);
            RequestUrl requestUrl = new("https://auth.frontierstore.net/auth");
            applicationDbContext.FDevApiAuthCodes.Add(new FDevApiAuthCode()
            {
                State = state,
                Code = codeVerifier,
            });
            return requestUrl.CreateAuthorizeUrl(
                clientId: ClientId,
                responseType: "code",
                redirectUri: RedirectUri,
                state: state,
                scope: "auth",
                codeChallenge: codeChallenge,
                codeChallengeMethod: "S256"
            );
        }

        public Task<Profile?> GetProfile(FDevApiCredentials apiCredentials)
        {
            return ApiCall<Profile>(apiCredentials, ProfileUrl);
        }

        private async Task<T?> ApiCall<T>(FDevApiCredentials apiCredentials, string url)
        {
            using HttpRequestMessage request = new(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue(apiCredentials.TokenType, apiCredentials.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Method = HttpMethod.Get;
            using HttpResponseMessage response = await HttpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                string bodyContent = await response.Content.ReadAsStringAsync();
                return (T?)JsonConvert.DeserializeObject(bodyContent, typeof(T));
            }
            return default;
        }
    }

    public class FDevAuthenticationResult
    {
        public TokenResponse TokenResponse { get; }
        public UserInfoResponse UserInfoResponse { get; }
        public long CustomerId { get; set; }
        public FDevApiCredentials Credentials { get; set; }
        public FDevAuthenticationResult(TokenResponse tokenResponse, UserInfoResponse userInfoResponse, long customerId)
        {
            TokenResponse = tokenResponse;
            UserInfoResponse = userInfoResponse;
            CustomerId = customerId;
            Credentials = new FDevApiCredentials(tokenResponse.TokenType, tokenResponse.AccessToken, tokenResponse.RefreshToken);
        }
    }

    public class FDevApiCredentials
    {
        public string TokenType { get; }
        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public FDevApiCredentials(string tokenType, string accessToken, string refreshToken)
        {
            TokenType = tokenType;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
