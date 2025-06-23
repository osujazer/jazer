using System;
using Jazer.Game.Configuration;
using Jazer.Game.Graphics;
using Jazer.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.Events;

namespace Jazer.Game.Overlays;

public partial class Toolbar : JazerOverlay, IKeyBindingHandler<GlobalAction>
{
    public const float HEIGHT = 60;

    protected override Container<Drawable> Content { get; }

    public readonly Bindable<ToolbarMode> ToolbarMode = new Bindable<ToolbarMode>();

    private readonly BindableBool showToolbar = new BindableBool();

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
    private void load(JazerConfigManager config)
    {
        config.BindWith(JazerSetting.ShowToolbar, showToolbar);

        Children =
        [
            new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = JazerColour.Gray700,
            }
        ];

        ToolbarMode.BindValueChanged(_ => toolbarModeChanged());
        showToolbar.BindValueChanged(_ => toolbarModeChanged());
        toolbarModeChanged();
    }

    protected override void PopIn() => this.ResizeHeightTo(HEIGHT, 400, Easing.OutExpo);

    protected override void PopOut() => this.ResizeHeightTo(0, 400, Easing.OutExpo);

    private void toolbarModeChanged()
    {
        Visible.Value = ToolbarMode.Value switch
        {
            Screens.ToolbarMode.Show => true,
            Screens.ToolbarMode.Hide => false,
            Screens.ToolbarMode.UserTriggered => showToolbar.Value,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool OnPressed(KeyBindingPressEvent<GlobalAction> e)
    {
        switch (e.Action)
        {
            case GlobalAction.ToggleToolbar:
                if (ToolbarMode.Value == Screens.ToolbarMode.UserTriggered)
                    showToolbar.Toggle();

                return true;
        }

        return false;
    }

    public void OnReleased(KeyBindingReleaseEvent<GlobalAction> e)
    {
    }
}
