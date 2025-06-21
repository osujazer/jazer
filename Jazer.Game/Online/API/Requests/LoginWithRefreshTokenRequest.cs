using System.Net.Http;
using Newtonsoft.Json;

namespace Jazer.Game.Online.API.Requests;

public class LoginWithRefreshTokenRequest : JazerJsonWebRequest<AuthToken>
{
    private readonly string refreshToken;

    public LoginWithRefreshTokenRequest(string refreshToken)
    {
        this.refreshToken = refreshToken;

        Method = HttpMethod.Post;
        Url = "https://jazer-api.tsunyoku.xyz/api/v1/users/login-refresh";
    }

    private LoginUserWithRefreshTokenRequest request => new LoginUserWithRefreshTokenRequest
    {
        RefreshToken = refreshToken
    };

    protected override void PrePerform()
    {
        AddRaw(JsonConvert.SerializeObject(request));
        AddHeader("Content-Type", "application/json");

        base.PrePerform();
    }

    private class LoginUserWithRefreshTokenRequest
    {
        [JsonProperty("refresh_token")]
        public required string RefreshToken { get; init; }
    }
}
