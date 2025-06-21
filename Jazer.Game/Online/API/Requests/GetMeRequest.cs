using Jazer.Game.Online.API.Responses;

namespace Jazer.Game.Online.API.Requests;

public class GetMeRequest : APIRequest<APIOwnUser>
{
    protected override string Target => @"users/me";
}
