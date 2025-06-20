using osu.Framework.iOS;
using Jazer.Game;

namespace Jazer.iOS
{
    /// <inheritdoc />
    public class AppDelegate : GameApplicationDelegate
    {
        /// <inheritdoc />
        protected override osu.Framework.Game CreateGame() => new JazerGame();
    }
}
