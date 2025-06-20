using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Osu.Objects;
using Jazer.Game.Rulesets.Osu.UI;
using JetBrains.Annotations;
using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osuTK;

namespace Jazer.Game.Tests.Visual;

public partial class TestSceneOsuPlayfield : JazerTestScene
{
    private readonly ManualFramedClock clock = new ManualFramedClock();

    private readonly TestPlayfield playfield;

    private readonly SpriteText objectCountText;

    public TestSceneOsuPlayfield()
    {
        var beatmap = new Beatmap();

        beatmap.HitObjects.AddRange([
            new HitCircle
            {
                StartTime = 1500,
                Position = new Vector2(100, 200),
            },
            new HitCircle
            {
                StartTime = 2500,
                Position = new Vector2(300, 250),
            }
        ]);

        AddRange([
            playfield = new TestPlayfield(beatmap)
            {
                RelativeSizeAxes = Axes.Both,
                Clock = clock,
                ProcessCustomClock = false,
            },
            objectCountText = new SpriteText
            {
                Anchor = Anchor.BottomRight,
                Origin = Anchor.BottomRight,
                Padding = new MarginPadding(10)
            }
        ]);
    }

    [Test]
    public void TestOsuPlayfield()
    {
        AddSliderStep("current time", 0.0, 5000.0, 0.0, time => clock.CurrentTime = time);
    }

    protected override void Update()
    {
        base.Update();

        clock.ProcessFrame();
    }

    protected override void UpdateAfterChildren()
    {
        base.UpdateAfterChildren();

        objectCountText.Text = $"Alive hitobjects: {playfield.AliveHitObjectCount}";
    }

    private partial class TestPlayfield : OsuPlayfield
    {
        public TestPlayfield([NotNull] Beatmap beatmap)
            : base(beatmap)
        {
        }

        public int AliveHitObjectCount => InternalChildren.Count;
    }
}
