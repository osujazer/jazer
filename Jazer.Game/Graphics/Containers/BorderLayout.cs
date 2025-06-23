using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Jazer.Game.Graphics.Containers;

public partial class BorderLayout : CompositeDrawable
{
    private readonly Container top = new Container
    {
        RelativeSizeAxes = Axes.X,
        AutoSizeAxes = Axes.Y,
    };

    private readonly Container bottom = new Container
    {
        RelativeSizeAxes = Axes.X,
        AutoSizeAxes = Axes.Y,
        Anchor = Anchor.BottomLeft,
        Origin = Anchor.BottomLeft,
    };

    private readonly Container left = new Container
    {
        RelativeSizeAxes = Axes.Y,
        AutoSizeAxes = Axes.X,
        Anchor = Anchor.TopRight,
        Origin = Anchor.TopRight,
    };

    private readonly Container right = new Container
    {
        RelativeSizeAxes = Axes.Y,
        AutoSizeAxes = Axes.X,
    };

    private readonly Container center = new Container
    {
        RelativeSizeAxes = Axes.Both,
    };

    public Drawable Top
    {
        set => top.Child = value;
    }

    public Drawable Bottom
    {
        set => bottom.Child = value;
    }

    public Drawable Left
    {
        set => right.Child = value;
    }

    public Drawable Right
    {
        set => left.Child = value;
    }

    public Drawable Center
    {
        set => center.Child = value;
    }

    public BorderLayout()
    {
        RelativeSizeAxes = Axes.Both;

        InternalChildren =
        [
            center,
            right,
            left,
            bottom,
            top,
        ];
    }

    protected override void Update()
    {
        base.Update();

        updateLayout();
    }

    private void updateLayout()
    {
        var padding = new MarginPadding
        {
            Top = top.DrawHeight,
            Bottom = bottom.DrawHeight,
        };

        right.Padding = padding;
        left.Padding = padding;

        center.Padding = padding with
        {
            Left = right.DrawWidth,
            Right = left.DrawWidth,
        };
    }
}
