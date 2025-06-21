using Jazer.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace Jazer.Game.Tests.Visual;

public partial class TestSceneJazerFont : JazerTestScene
{
    public TestSceneJazerFont()
    {
        Child = new FillFlowContainer
        {
            RelativeSizeAxes = Axes.Both,
            Direction = FillDirection.Vertical,
            Padding = new MarginPadding(20),
            Spacing = new Vector2(10),
            Children =
            [
                new JazerSpriteText
                {
                    Text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                    Font = JazerFont.Get(size: 24)
                },
                new JazerSpriteText
                {
                    Text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                    Font = JazerFont.Get(size: 24, weight: FontWeight.Medium)
                },
                new JazerSpriteText
                {
                    Text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                    Font = JazerFont.Get(size: 24, weight: FontWeight.SemiBold)
                },
                new JazerSpriteText
                {
                    Text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                    Font = JazerFont.Get(size: 24, weight: FontWeight.Bold)
                }
            ]
        };
    }
}
