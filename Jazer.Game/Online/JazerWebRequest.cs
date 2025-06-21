using osu.Framework.IO.Network;

namespace Jazer.Game.Online;

public class JazerWebRequest : WebRequest
{
    public JazerWebRequest(string uri)
        : base(uri)
    {
    }

    public JazerWebRequest()
    {
    }

    protected override string UserAgent => "jazer";
}
