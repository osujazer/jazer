using Newtonsoft.Json;

namespace Jazer.Game.Online.API.Responses;

public class APIOwnUser : APIUser
{
    [JsonProperty("email")]
    public required string Email { get; init; }
}
