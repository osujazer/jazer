using Newtonsoft.Json;

namespace Jazer.Game.Online.API.Responses;

public class ErrorResponse
{
    [JsonProperty("errors")]
    public string[] Errors { get; init; } = [];
}
