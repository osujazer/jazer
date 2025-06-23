using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;

namespace Jazer.Game.Overlays;

public abstract partial class JazerOverlay : Container
{
    public readonly BindableBool Visible;

    protected JazerOverlay(bool startHidden = false)
    {
        Visible = new BindableBool(!startHidden);
    }

    protected abstract void PopIn();

    protected abstract void PopOut();

    public override void Show() => Visible.Value = true;

    public override void Hide() => Visible.Value = false;

    public void Toggle() => Visible.Toggle();

    protected override void LoadAsyncComplete()
    {
        base.LoadAsyncComplete();

        Visible.BindValueChanged(visible =>
        {
            if (visible.NewValue)
                PopIn();
            else
                PopOut();
        }, true);
        FinishTransforms(true);
    }
}
