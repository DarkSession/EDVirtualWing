using ED_Virtual_Wing.Data;
using ED_Virtual_Wing.Models;
using IdentityModel;
using IdentityModel.Client;
using System.Security.Cryptography;
using System.Text;

namespace ED_Virtual_Wing.FDevApi
{
    public class FDevApi
    {
        private static HttpClient HttpClient { get; set; } = new();
        private string ClientId { get; }
        private string RedirectUri { get; }

        private IConfiguration Configuration { get; }

        public FDevApi(IConfiguration configuration)
        {
            Configuration = configuration;
            ClientId = configuration["EDVW:FDevClientId"];
            RedirectUri = Configuration["EDVW:FDevAuthReturnUrl"];
        }

        public async Task<FDevApiResult?> AuthenticateUser(string userCode, FDevApiAuthCode fdevApiAuthCode)
        {
            // If this request fails with an error 500, then we probably have a compatibility issue.
            // It was tested and worked with IdentityModel 5.2
            TokenResponse tokenResponse = await HttpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = "https://auth.frontierstore.net/token",
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
                    Address = "https://auth.frontierstore.net/me",
                    Token = tokenResponse.AccessToken,
                });
                if (!userInfoResponse.IsError)
                {
                    long customerId = Convert.ToInt64(userInfoResponse.TryGet("customer_id") ?? "0");
                    return new FDevApiResult(tokenResponse, userInfoResponse, customerId);
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
    }

    public class FDevApiResult
    {
        public TokenResponse TokenResponse { get; }
        public UserInfoResponse UserInfoResponse { get; }
        public long CustomerId { get; set; }
        public FDevApiResult(TokenResponse tokenResponse, UserInfoResponse userInfoResponse, long customerId)
        {
            TokenResponse = tokenResponse;
            UserInfoResponse = userInfoResponse;
            CustomerId = customerId;
        }
    }
}
