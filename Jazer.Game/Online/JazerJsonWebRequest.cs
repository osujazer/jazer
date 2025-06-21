using osu.Framework.IO.Network;

namespace Jazer.Game.Online;

public class JazerJsonWebRequest<T> : JsonWebRequest<T>
{
    public JazerJsonWebRequest(string uri)
        : base(uri)
    {
    }

    public JazerJsonWebRequest()
    {
    }

    protected override string UserAgent => "jazer";
}
