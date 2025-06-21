using System;
using System.Net.Http;
using System.Net.Sockets;
using Jazer.Game.Online.API.Requests;
using Jazer.Game.Online.API.Responses;
using Newtonsoft.Json;
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
            Token.Value = null;

            var throwableException = ex;

            try
            {
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(loginRequest.GetResponseString() ?? string.Empty);

                if (errorResponse is not null)
                    throwableException = new APIException(errorResponse.Errors, ex);
            }
            catch
            {
            }

            throw throwableException;
        }

        Token.Value = loginRequest.ResponseObject;
    }

    internal bool AuthenticateWithRefreshToken(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Missing refresh token");

        using var loginWithRefreshTokenRequest = new LoginWithRefreshTokenRequest(refreshToken);

        try
        {
            loginWithRefreshTokenRequest.Perform();

            Token.Value = loginWithRefreshTokenRequest.ResponseObject;

            return true;
        }
        catch (SocketException)
        {
            return false;
        }
        catch (HttpRequestException)
        {
            return false;
        }
        catch (Exception)
        {
            Token.Value = null;

            return false;
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

        if (!string.IsNullOrWhiteSpace(Token.Value?.RefreshToken))
            AuthenticateWithRefreshToken(Token.Value!.RefreshToken);

        return Token.Value?.IsValid == true;
    }
}
