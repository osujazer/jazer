using osu.Framework.Allocation;
using NUnit.Framework;

namespace Jazer.Game.Tests.Visual
{
    [TestFixture]
    public partial class TestSceneJazerGame : JazerTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        [BackgroundDependencyLoader]
        private void load()
        {
            AddGame(new JazerGame());
        }
    }
}
