using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Osu.Objects;

namespace Jazer.Game.Rulesets.Osu.Gameplay;

public class HitCircleState : OsuHitObjectState<HitCircle>
{
    public HitCircleState(HitCircle hitObject, Beatmap beatmap)
        : base(hitObject, beatmap)
    {
    }
}
