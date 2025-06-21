using System;
using Jazer.Game.Online.API.Requests;
using JetBrains.Annotations;
using osu.Framework.Bindables;

namespace Jazer.Game.Online.API;

public class Auth
{
    public readonly Bindable<AuthToken> Token = new Bindable<AuthToken>();

    public string TokenString
    {
        get => Token.Value?.ToString();
        set => Token.Value = string.IsNullOrWhiteSpace(value) ? null : AuthToken.Parse(value);
    }

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

    [CanBeNull]
    internal string RetrieveAccessToken()
    {
        lock (access_token_lock)
        {
            return ensureAccessToken()
                ? Token.Value.AccessToken
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
