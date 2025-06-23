using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Screens;

namespace Jazer.Game.Screens;

public partial class JazerScreenStack : ScreenStack
{
    [Resolved(canBeNull: true)]
    private Bindable<ToolbarMode>? toolbarMode { get; set; } = null!;

    public JazerScreenStack()
    {
        ScreenPushed += screenPushed;
    }

    private void screenPushed(IScreen prev, IScreen next)
    {
        if (toolbarMode != null && next is JazerScreen jazerScreen)
            toolbarMode.Value = jazerScreen.ToolbarMode;
    }
}
