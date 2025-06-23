using System.Net.Http;
using Newtonsoft.Json;

namespace Jazer.Game.Online.API.Requests;

public class LoginRequest : JazerJsonWebRequest<AuthToken>
{
    private readonly string username;
    private readonly string password;

    public LoginRequest(string username, string password)
    {
        this.username = username;
        this.password = password;

        Method = HttpMethod.Post;
        ContentType = "application/json";
        Url = "https://jazer-api.tsunyoku.xyz/api/v1/users/login";
    }

    private LoginUserRequest request => new LoginUserRequest
    {
        Username = username,
        Password = password
    };

    protected override void PrePerform()
    {
        AddRaw(JsonConvert.SerializeObject(request));

        base.PrePerform();
    }

    private class LoginUserRequest
    {
        [JsonProperty("username")]
        public required string Username { get; init; }

        [JsonProperty("password")]
        public required string Password { get; init; }
    }
}
