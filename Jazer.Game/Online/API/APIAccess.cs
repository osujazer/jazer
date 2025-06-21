using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Jazer.Game.Configuration;
using Jazer.Game.Online.API.Requests;
using Jazer.Game.Online.API.Responses;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Framework.Development;
using osu.Framework.Graphics;

namespace Jazer.Game.Online.API;

public partial class APIAccess : Component
{
    private readonly JazerGameBase game;
    private readonly JazerConfigManager config;
    private readonly Auth auth;

    private readonly Queue<APIRequest> queue = new Queue<APIRequest>();

    public string ProvidedUsername { get; private set; }

    private string password { get; set; }

    private readonly Bindable<APIState> state = new Bindable<APIState>();

    public IBindable<APIState> State => state;

    private Bindable<APIUser> localUser { get; } = new Bindable<APIUser>(guestUser);

    public IBindable<APIUser> LocalUser => localUser;

    public string AccessToken => auth.RetrieveAccessToken();

    [CanBeNull]
    public Exception LastLoginError { get; private set; }

    protected bool HasLogin => auth.Token.Value != null ||
                               (!string.IsNullOrEmpty(ProvidedUsername) && !string.IsNullOrEmpty(password));

    private readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();

    private int failureCount;

    public APIAccess(JazerGameBase game, JazerConfigManager config)
    {
        this.game = game;
        this.config = config;

        auth = new Auth();

        ProvidedUsername = config.Get<string>(JazerSetting.Username);

        auth.TokenString = config.Get<string>(JazerSetting.AuthToken);
        auth.Token.ValueChanged += onTokenChanged;

        if (HasLogin)
        {
            setPlaceholderUser();
            state.Value = APIState.Connecting;
        }

        var thread = new Thread(run)
        {
            IsBackground = true,
        };

        thread.Start();
    }

    public new void Schedule(Action action) => base.Schedule(action);

    public void Perform(APIRequest request)
    {
        try
        {
            request.AttachAPI(this);
            request.Perform();
        }
        catch (Exception ex)
        {
            request.Fail(ex);
        }
    }

    public Task PerformAsync(APIRequest request) =>
        Task.Factory.StartNew(() => Perform(request), TaskCreationOptions.LongRunning);

    public void Login(string username, string password)
    {
        Debug.Assert(State.Value == APIState.Offline);

        ProvidedUsername = username;
        this.password = password;
    }

    public void Logout()
    {
        password = null;
        auth.Clear();

        Schedule(() =>
        {
            localUser.Value = guestUser;
        });

        state.Value = APIState.Offline;
        flushQueue();
    }

    public void Queue(APIRequest request)
    {
        lock (queue)
        {
            request.AttachAPI(this);

            if (state.Value == APIState.Offline)
            {
                request.Fail(new WebException(@"User not logged in"));
                return;
            }

            queue.Enqueue(request);
        }
    }

    private void run()
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            if (state.Value == APIState.Failing)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }

            if (!HasLogin)
            {
                state.Value = APIState.Offline;
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
                continue;
            }

            if (state.Value != APIState.Online)
            {
                attemptConnect();

                if (state.Value != APIState.Online)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    continue;
                }
            }

            if (auth.RetrieveAccessToken() is null)
            {
                Logout();
                continue;
            }

            processQueuedRequests();
            Thread.Sleep(50);
        }
    }

    private void attemptConnect()
    {
        if (localUser.IsDefault)
            Scheduler.Add(setPlaceholderUser, false);

        config.SetValue(
            JazerSetting.Username,
            config.Get<bool>(JazerSetting.SaveUsername) ? ProvidedUsername : string.Empty);

        if (auth.RetrieveAccessToken() is null && HasLogin)
        {
            state.Value = APIState.Connecting;
            LastLoginError = null;

            try
            {
                auth.AuthenticateWithLogin(ProvidedUsername, password);
            }
            catch (WebRequestFlushedException)
            {
                return;
            }
            catch (Exception e)
            {
                LastLoginError = e;

                Logout();
                return;
            }
        }

        var userReq = new GetMeRequest();

        userReq.Failure += ex =>
        {
            if (ex is WebException webException && webException.Message == @"Unauthorized")
            {
                Logout();
            }
            else if (ex is not WebRequestFlushedException)
            {
                state.Value = APIState.Failing;
            }
        };

        userReq.Success += me =>
        {
            Debug.Assert(ThreadSafety.IsUpdateThread);

            localUser.Value = me;
            state.Value = APIState.Online;
            failureCount = 0;
        };

        if (!handleRequest(userReq))
        {
            state.Value = APIState.Failing;
            return;
        }

        while (State.Value == APIState.Connecting && !cancellationToken.IsCancellationRequested)
            Thread.Sleep(500);
    }

    private void processQueuedRequests()
    {
        while (true)
        {
            APIRequest req;

            lock (queue)
            {
                if (queue.Count == 0)
                    return;

                req = queue.Dequeue();
            }

            handleRequest(req);
        }
    }

    private bool handleRequest(APIRequest req)
    {
        try
        {
            req.AttachAPI(this);
            req.Perform();

            if (req.CompletionState != APIRequestCompletionState.Completed)
                return false;

            failureCount = 0;
            return true;
        }
        catch (HttpRequestException)
        {
            handleFailure();
            return false;
        }
        catch (SocketException)
        {
            handleFailure();
            return false;
        }
        catch (WebException ex)
        {
            handleWebException(ex);
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private void handleWebException(WebException we)
    {
        HttpStatusCode statusCode = (we.Response as HttpWebResponse)?.StatusCode
                                    ?? (we.Status == WebExceptionStatus.UnknownError ? HttpStatusCode.NotAcceptable : HttpStatusCode.RequestTimeout);

        switch (we.Message)
        {
            case "Unauthorized":
            case "Forbidden":
                statusCode = HttpStatusCode.Unauthorized;
                break;
        }

        switch (statusCode)
        {
            case HttpStatusCode.Unauthorized:
                Logout();
                break;

            case HttpStatusCode.RequestTimeout:
                handleFailure();
                break;
        }
    }

    private void handleFailure()
    {
        failureCount++;

        if (failureCount >= 3)
        {
            state.Value = APIState.Failing;
            flushQueue();
        }
    }

    private void flushQueue(bool failOldRequests = true)
    {
        lock (queue)
        {
            var oldQueueRequests = queue.ToArray();

            queue.Clear();

            if (failOldRequests)
            {
                foreach (var req in oldQueueRequests)
                    req.Fail(new WebRequestFlushedException(state.Value));
            }
        }
    }

    private void setPlaceholderUser()
    {
        if (!localUser.IsDefault)
            return;

        localUser.Value = new APIUser
        {
            Id = -1,
            Username = ProvidedUsername,
            CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
        };
    }

    private static APIUser guestUser => new APIUser
    {
        Id = -1,
        Username = "Guest",
        CreatedAt = DateTimeOffset.UtcNow.AddDays(-1),
    };

    private void onTokenChanged(ValueChangedEvent<AuthToken> e)
        => config.SetValue(
            JazerSetting.AuthToken,
            config.Get<bool>(JazerSetting.SavePassword) ? auth.TokenString : string.Empty);

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        flushQueue();
        cancellationToken.Cancel();
    }

    private class WebRequestFlushedException(APIState state)
        : Exception($@"Request failed from flush operation (state {state})");
}


public enum APIState
{
    Offline,
    Failing,
    Connecting,
    Online
}
