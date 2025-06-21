using System.Net.Http;
using Newtonsoft.Json;

namespace Jazer.Game.Online.API.Requests;

public class RegisterRequest : JazerWebRequest
{
    private readonly string username;
    private readonly string email;
    private readonly string password;

    public RegisterRequest(string username, string email, string password)
    {
        this.username = username;
        this.email = email;
        this.password = password;

        Method = HttpMethod.Post;
        Url = "https://jazer-api.tsunyoku.xyz/api/v1/users";
    }

    private RegisterUserRequest request => new RegisterUserRequest
    {
        Username = username,
        Email = email,
        Password = password
    };

    protected override void PrePerform()
    {
        AddRaw(JsonConvert.SerializeObject(request));
        AddHeader("Content-Type", "application/json");

        base.PrePerform();
    }

    private class RegisterUserRequest
    {
        [JsonProperty("username")]
        public required string Username { get; init; }

        [JsonProperty("email")]
        public required string Email { get; init; }

        [JsonProperty("password")]
        public required string Password { get; init; }
    }
}
