using System;
using System.Diagnostics;
using Jazer.Game.Online.API.Responses;
using Newtonsoft.Json;
using osu.Framework.IO.Network;

namespace Jazer.Game.Online.API;

public abstract class APIRequest<T> : APIRequest where T : class
{
    protected override WebRequest CreateWebRequest() => new JazerJsonWebRequest<T>(Uri);

    public T? Response { get; private set; }

    public new event APISuccessHandler<T>? Success;

    protected APIRequest()
    {
        base.Success += () => Success?.Invoke(Response!);
    }

    protected override void PostProcess()
    {
        base.PostProcess();

        if (WebRequest is not null)
            Response = ((JazerJsonWebRequest<T>)WebRequest).ResponseObject;

        if (Response is null)
            TriggerFailure(new ArgumentNullException(nameof(Response)));
    }

    internal void TriggerSuccess(T response)
    {
        Response = response;
        TriggerSuccess();
    }
}

public abstract class APIRequest
{
    protected abstract string Target { get; }

    protected virtual WebRequest CreateWebRequest() => new JazerWebRequest(Uri);

    protected virtual string Uri => $@"https://jazer-api.tsunyoku.xyz/api/v1/{Target}";

    protected IAPIAccess? API;

    protected WebRequest? WebRequest;

    public event APISuccessHandler? Success;

    public event APIFailureHandler? Failure;

    private readonly object completionStateLock = new object();

    public APIRequestCompletionState CompletionState { get; private set; }

    private bool isFailing
    {
        get
        {
            lock (completionStateLock)
                return CompletionState == APIRequestCompletionState.Failed;
        }
    }

    public void AttachAPI(IAPIAccess api)
    {
        API = api;
    }

    public void Perform()
    {
        Debug.Assert(API is not null);

        if (isFailing)
            return;

        WebRequest = CreateWebRequest();
        WebRequest.Failed += Fail;
        WebRequest.AllowRetryOnTimeout = false;

        if (!string.IsNullOrEmpty(API.AccessToken))
            WebRequest.AddHeader(@"Authorization", $@"Bearer {API.AccessToken}");

        if (isFailing)
            return;

        try
        {
            WebRequest.Perform();
        }
        catch (OperationCanceledException)
        {
        }

        if (isFailing)
            return;

        PostProcess();

        if (isFailing)
            return;

        TriggerSuccess();
    }

    protected virtual void PostProcess()
    {
    }

    internal void TriggerSuccess()
    {
        Debug.Assert(API is not null);

        lock (completionStateLock)
        {
            if (CompletionState != APIRequestCompletionState.Waiting)
                return;

            CompletionState = APIRequestCompletionState.Completed;
        }

        API.Schedule(() => Success?.Invoke());
    }

    internal void TriggerFailure(Exception e)
    {
        Debug.Assert(API is not null);

        lock (completionStateLock)
        {
            if (CompletionState != APIRequestCompletionState.Waiting)
                return;

            CompletionState = APIRequestCompletionState.Failed;
        }

        API.Schedule(() => Failure?.Invoke(e));
    }

    public void Cancel() => Fail(new OperationCanceledException(@"Request cancelled"));

    public void Fail(Exception e)
    {
        lock (completionStateLock)
        {
            if (CompletionState != APIRequestCompletionState.Waiting)
                return;

            WebRequest?.Abort();

            if (e is not OperationCanceledException)
            {
                try
                {
                    string? responseString = WebRequest?.GetResponseString();

                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString ?? string.Empty);

                    if (errorResponse is not null)
                        e = new APIException(errorResponse.Errors, e);
                }
                catch
                {
                }
            }

            TriggerFailure(e);
        }
    }
}

public enum APIRequestCompletionState
{
    Waiting,
    Completed,
    Failed
}

public delegate void APIFailureHandler(Exception e);

public delegate void APISuccessHandler();

public delegate void APISuccessHandler<in T>(T content);
