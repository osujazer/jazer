using osu.Framework.Platform;
using osu.Framework;
using Jazer.Game;

namespace Jazer.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"Jazer"))
            using (osu.Framework.Game game = new JazerGame())
                host.Run(game);
        }
    }
}
