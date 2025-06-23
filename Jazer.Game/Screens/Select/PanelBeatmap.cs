using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace Jazer.Game.Screens.Select;

public partial class PanelBeatmap : Panel
{
    [BackgroundDependencyLoader]
    private void load()
    {
        Background = new Box
        {
            RelativeSizeAxes = Axes.Both,
            Colour = Color4.Gray,
        };
    }
}
