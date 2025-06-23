using Jazer.Game.Overlays;
using Jazer.Game.Screens;
using osu.Framework.Allocation;

namespace Jazer.Game
{
    public partial class JazerGame : JazerGameBase
    {
        private JazerScreenStack screenStack = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(new ToolbarContainer
            {
                Child = screenStack = new JazerScreenStack()
            });
        }
    }
}
