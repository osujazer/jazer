using System;
using System.Threading;
using System.Threading.Tasks;
using Jazer.Game.Online.API.Responses;
using osu.Framework.Bindables;

namespace Jazer.Game.Online.API;

public interface IAPIAccess
{
    IBindable<APIUser> LocalUser { get; }

    string? AccessToken { get; }

    string ProvidedUsername { get; }

    Exception? LastLoginError { get; }

    IBindable<APIState> State { get; }

    void Queue(APIRequest request);

    void Perform(APIRequest request);

    Task PerformAsync(APIRequest request, CancellationToken cancellationToken = default);

    void Login(string username, string password);

    void Logout();

    ErrorResponse? Register(string username, string email, string password);

    internal void Schedule(Action action);
}
