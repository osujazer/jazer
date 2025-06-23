using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace Jazer.Game.Screens;

public partial class JazerScreenStack : ScreenStack
{
    public readonly Bindable<ToolbarMode> ToolbarMode = new Bindable<ToolbarMode>();

    public JazerScreenStack()
    {
        ScreenPushed += screenPushed;
    }

    private void screenPushed(IScreen prev, IScreen next)
    {
        if (next is JazerScreen jazerScreen)
            ToolbarMode.Value = jazerScreen.ToolbarMode;
    }
}
