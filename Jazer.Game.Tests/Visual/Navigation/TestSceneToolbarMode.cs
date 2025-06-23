using System.Linq;
using Jazer.Game.Overlays;
using Jazer.Game.Screens;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Testing;
using osu.Framework.Utils;

namespace Jazer.Game.Tests.Visual.Navigation;

public partial class TestSceneToolbarMode : JazerTestScene
{
    private JazerGame game = null!;

    private JazerScreenStack screenStack => game.ChildrenOfType<JazerScreenStack>().First();
    private Toolbar toolbar => game.ChildrenOfType<Toolbar>().First();

    [BackgroundDependencyLoader]
    private void load()
    {
        AddGame(game = new JazerGame());
    }

    [SetUpSteps]
    public void Setup()
    {
        AddStep("exit all screens", () =>
        {
            while (screenStack.CurrentScreen != null)
                screenStack.Exit();
        });
    }

    [Test]
    public void TestToolbarMode()
    {
        AddStep("add screen with toolbar", () => screenStack.Push(new TestScreen(ToolbarMode.Show)));
        AddUntilStep("toolbar finished transform", () => !toolbar.Transforms.Any());
        AddAssert("toolbar is visible", () => Precision.AlmostEquals(toolbar.Height, Toolbar.HEIGHT));
        AddStep("add screen without toolbar", () => screenStack.Push(new TestScreen(ToolbarMode.Hide)));
        AddUntilStep("toolbar finished transform", () => !toolbar.Transforms.Any());
        AddAssert("toolbar is hidden", () => Precision.AlmostEquals(toolbar.Height, 0));
    }

    private partial class TestScreen(ToolbarMode toolbarMode) : JazerScreen
    {
        public override ToolbarMode ToolbarMode => toolbarMode;
    }
}
