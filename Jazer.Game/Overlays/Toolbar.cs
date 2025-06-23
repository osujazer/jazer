using Jazer.Game.Graphics;
using Jazer.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace Jazer.Game.Overlays;

public partial class Toolbar : JazerOverlay
{
    public const float HEIGHT = 60;

    protected override Container<Drawable> Content { get; }

    public readonly Bindable<ToolbarMode> ToolbarMode = new Bindable<ToolbarMode>();

    public Toolbar()
    {
        RelativeSizeAxes = Axes.X;
        Height = HEIGHT;

        InternalChild = Content = new Container
        {
            RelativeSizeAxes = Axes.X,
            Height = Height,
            Anchor = Anchor.BottomLeft,
            Origin = Anchor.BottomLeft,
        };
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        Children =
        [
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = JazerColour.Gray700,
            }
        ];

        ToolbarMode.BindValueChanged(mode => Visible.Value = mode.NewValue == Screens.ToolbarMode.Show, true);
    }

    protected override void PopIn() => this.ResizeHeightTo(HEIGHT, 400, Easing.OutExpo);

    protected override void PopOut() => this.ResizeHeightTo(0, 400, Easing.OutExpo);
}
