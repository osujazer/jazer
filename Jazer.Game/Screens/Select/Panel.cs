using Jazer.Game.Graphics.Carousel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace Jazer.Game.Screens.Select;

public partial class Panel : PoolableDrawable, ICarouselPanel
{
    public BindableBool Selected { get; } = new BindableBool();
    public BindableBool Expanded { get; } = new BindableBool();
    public BindableBool KeyboardSelected { get; } = new BindableBool();

    public double DrawYPosition { get; set; }
    public CarouselItem? Item { get; set; }

    private Container backgroundContainer = null!;

    [Resolved(canBeNull: true)]
    private BeatmapCarousel? carousel { get; set; }

    protected Drawable Background
    {
        set => backgroundContainer.Child = value;
    }

    public Container TopLevelContent { get; private set; } = null!;

    public Container Content { get; private set; } = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        Anchor = Anchor.TopRight;
        Origin = Anchor.TopRight;

        RelativeSizeAxes = Axes.X;
        Height = CarouselItem.DEFAULT_HEIGHT;

        InternalChild = TopLevelContent = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Masking = true,
            EdgeEffect = new EdgeEffectParameters
            {
                Type = EdgeEffectType.Shadow,
                Radius = 10,
                Colour = Color4.Black.Opacity(0.25f),
            },
            Children =
            [
                new Box { RelativeSizeAxes = Axes.Both, Alpha = 0, AlwaysPresent = true },
                backgroundContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
                Content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                }
            ]
        };
    }

    protected override void PrepareForUse()
    {
        base.PrepareForUse();

        this.FadeIn(300, Easing.OutExpo);
    }

    public void Activated()
    {
    }

    protected override bool OnClick(ClickEvent e)
    {
        carousel?.Activate(Item!);
        return true;
    }
}
