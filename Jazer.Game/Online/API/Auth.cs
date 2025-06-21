#nullable enable

using System;
using Jazer.Game.Online.API.Requests;
using osu.Framework.Bindables;

namespace Jazer.Game.Online.API;

public class Auth(AuthToken? token)
{
    public readonly Bindable<AuthToken?> Token = new Bindable<AuthToken?>(token);

    public string? TokenString => Token.Value?.ToString();

    internal void AuthenticateWithLogin(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Missing username or password");

        using var loginRequest = new LoginRequest(username, password);

        try
        {
            loginRequest.Perform();
        }
        catch (Exception ex)
        {
            lock (access_token_lock)
            {
                Token.Value = null;
            }

            // TODO: better than this

            throw;
        }

        lock (access_token_lock)
        {
            Token.Value = loginRequest.ResponseObject;
        }
    }

    private static readonly object access_token_lock = new object();

    internal string? RetrieveAccessToken()
    {
        lock (access_token_lock)
        {
            return ensureAccessToken()
                ? Token.Value!.AccessToken
                : null;
        }
    }

    internal void Clear()
    {
        lock (access_token_lock)
            Token.Value = null;
    }

    private bool ensureAccessToken()
    {
        if (Token.Value?.IsValid == true)
            return true;

        // TODO: refresh token

        return false;
    }
}
