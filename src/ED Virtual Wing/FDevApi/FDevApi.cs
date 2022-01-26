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
        private static string ClientId { get; } = Environment.GetEnvironmentVariable("FRONTIER_AUTH_CLIENT_ID") ?? string.Empty;
        private static string RedirectUri { get; } = (Environment.GetEnvironmentVariable("EDVW_HTTP_ORIGIN") ?? string.Empty) + "/auth";

        public async Task<FDevApiResult?> AuthenticateUser(string userCode, string verifierCode)
        {
            TokenResponse tokenResponse = await HttpClient.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = "https://auth.frontierstore.net/token",
                ClientId = ClientId,
                Code = userCode,
                RedirectUri = RedirectUri,
                CodeVerifier = verifierCode,
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
            using SHA256 sha256 = SHA256.Create();
            string state = CryptoRandom.CreateUniqueId(32);
            string codeVerifier = CryptoRandom.CreateUniqueId(32);
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
