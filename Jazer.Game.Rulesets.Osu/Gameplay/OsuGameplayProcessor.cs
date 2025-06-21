using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Gameplay;
using Jazer.Game.Rulesets.Objects;
using Jazer.Game.Rulesets.Osu.Objects;

namespace Jazer.Game.Rulesets.Osu.Gameplay;

public class OsuGameplayProcessor : GameplayProcessor<OsuHitObjectState>
{
    public OsuGameplayProcessor(Beatmap beatmap)
        : base(beatmap)
    {
    }

    protected override OsuHitObjectState CreateHitObjectState(HitObject hitObject, Beatmap beatmap) => hitObject switch
    {
        HitCircle hitCircle => new HitCircleState(hitCircle, beatmap),
        _ => throw new ArgumentOutOfRangeException(nameof(hitObject), hitObject, null)
    };

    protected override void ProcessFrame()
    {
    }
}
