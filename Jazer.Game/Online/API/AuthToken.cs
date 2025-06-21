using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Jazer.Game.Online.API;

public class AuthToken
{
    [JsonProperty("access_token")]
    public required string AccessToken { get; init; }

    [JsonProperty("refresh_token")]
    public required string RefreshToken { get; init; }

    [JsonProperty("expires_at")]
    public required DateTimeOffset ExpiresAt { get; init; }

    public TimeSpan ExpiresIn => ExpiresAt - DateTimeOffset.UtcNow;

    public bool IsValid => !string.IsNullOrWhiteSpace(AccessToken) && ExpiresIn.TotalSeconds > 30;

    public override string ToString() => $@"{AccessToken}|{ExpiresAt.ToString(NumberFormatInfo.InvariantInfo)}|{RefreshToken}";

    public static AuthToken Parse(string value)
    {
        try
        {
            string[] parts = value.Split('|');

            return new AuthToken
            {
                AccessToken = parts[0],
                ExpiresAt = DateTimeOffset.Parse(parts[1], NumberFormatInfo.InvariantInfo),
                RefreshToken = parts[2]
            };
        }
        catch
        {
        }

        return null;
    }
}
