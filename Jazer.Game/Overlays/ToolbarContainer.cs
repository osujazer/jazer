using Jazer.Game.Graphics.Containers;
using Jazer.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Jazer.Game.Overlays;

public partial class ToolbarContainer : Container
{
    protected override Container<Drawable> Content { get; }

    [Cached]
    public readonly Bindable<ToolbarMode> ToolbarMode = new Bindable<ToolbarMode>();

    public readonly Toolbar Toolbar;

    public ToolbarContainer()
    {
        RelativeSizeAxes = Axes.Both;

        InternalChild = new BorderLayout
        {
            RelativeSizeAxes = Axes.Both,
            Top = Toolbar = new Toolbar
            {
                ToolbarMode = { BindTarget = ToolbarMode }
            },
            Center = Content = new Container
            {
                RelativeSizeAxes = Axes.Both
            }
        };
    }
}
