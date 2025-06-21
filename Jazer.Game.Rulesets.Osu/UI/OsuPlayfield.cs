using Jazer.Game.Rulesets.Gameplay;
using Jazer.Game.Rulesets.Objects;
using Jazer.Game.Rulesets.Objects.Drawables;
using Jazer.Game.Rulesets.Osu.Objects;
using Jazer.Game.Rulesets.Osu.Objects.Drawables;
using Jazer.Game.Rulesets.UI;

namespace Jazer.Game.Rulesets.Osu.UI;

public partial class OsuPlayfield : Playfield
{
    public OsuPlayfield(GameplayProcessor processor)
        : base(processor)
    {
    }

    protected override DrawableHitObject CreateDrawableFor(HitObject hitObject) => hitObject switch
    {
        HitCircle hitCircle => new DrawableHitCircle(hitCircle),
        _ => throw new ArgumentOutOfRangeException(nameof(hitObject), hitObject, null)
    };
}
