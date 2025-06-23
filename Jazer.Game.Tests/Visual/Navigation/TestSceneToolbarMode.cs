using Jazer.Game.Configuration;
using Jazer.Game.Overlays;
using Jazer.Game.Screens;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;

namespace Jazer.Game.Tests.Visual.Navigation;

public partial class TestSceneToolbarMode : JazerTestScene
{
    private readonly ToolbarContainer toolbarContainer;
    private readonly GlobalActionContainer globalActionContainer;
    private readonly JazerScreenStack screenStack;

    private Toolbar toolbar => toolbarContainer.Toolbar;

    [Resolved]
    private JazerConfigManager config { get; set; } = null!;

    public TestSceneToolbarMode()
    {
        Add(globalActionContainer = new GlobalActionContainer()
        {
            Child = toolbarContainer = new ToolbarContainer
            {
                Child = screenStack = new JazerScreenStack()
            }
        });
    }

    [BackgroundDependencyLoader]
    private void load()
    {
    }

    [SetUpSteps]
    public void Setup()
    {
        AddStep("exit all screens", () =>
        {
            while (screenStack.CurrentScreen != null)
                screenStack.Exit();
        });
        AddStep("reset config", () => config.SetValue(JazerSetting.ShowToolbar, true));
    }

    [Test]
    public void TestToolbarMode()
    {
        AddStep("add screen with toolbar", () => screenStack.Push(new TestScreen(ToolbarMode.Show)));
        AddAssert("toolbar is visible", () => toolbar.Visible.Value);
        AddStep("add screen without toolbar", () => screenStack.Push(new TestScreen(ToolbarMode.Hide)));
        AddAssert("toolbar is hidden", () => !toolbar.Visible.Value);
        AddStep("add screen with manual toolbar", () => screenStack.Push(new TestScreen(ToolbarMode.UserTriggered)));

        AddAssert("toolbar is visible", () => toolbar.Visible.Value);
        AddStep("toggle toolbar", () => triggerAction(GlobalAction.ToggleToolbar));
        AddAssert("toolbar is hidden", () => !toolbar.Visible.Value);
    }

    private void triggerAction(GlobalAction action)
    {
        globalActionContainer.TriggerPressed(action);
        globalActionContainer.TriggerReleased(action);
    }

    private partial class TestScreen : JazerScreen
    {
        public override ToolbarMode ToolbarMode { get; }

        public TestScreen(ToolbarMode toolbarMode)
        {
            ToolbarMode = toolbarMode;

            Padding = new MarginPadding(10);
            AddInternal(new Box { RelativeSizeAxes = Axes.Both, Alpha = 0.25f });
        }
    }
}
