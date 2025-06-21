using System;
using Newtonsoft.Json;

namespace Jazer.Game.Online.API.Responses;

public class APIUser : IEquatable<APIUser>
{
    [JsonProperty("id")]
    public required int Id { get; init; }

    [JsonProperty("username")]
    public required string Username { get; init; }

    [JsonProperty("created_at")]
    public required DateTimeOffset CreatedAt { get; init; }

    public override string ToString() => Username;

    public bool Equals(APIUser other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        return Equals((APIUser)obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}
