using Jazer.Game.Graphics.Containers;
using Jazer.Game.Overlays;
using Jazer.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;

namespace Jazer.Game
{
    public partial class JazerGame : JazerGameBase
    {
        private JazerScreenStack screenStack = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Toolbar toolbar;

            Add(new BorderLayout
            {
                RelativeSizeAxes = Axes.Both,
                Top = toolbar = new Toolbar(),
                Center = screenStack = new JazerScreenStack
                {
                    ToolbarMode = { BindTarget = toolbar.ToolbarMode }
                }
            });
        }
    }
}
