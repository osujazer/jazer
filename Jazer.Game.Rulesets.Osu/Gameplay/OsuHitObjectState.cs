using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Gameplay;
using Jazer.Game.Rulesets.Osu.Objects;

namespace Jazer.Game.Rulesets.Osu.Gameplay;

public abstract class OsuHitObjectState : HitObjectState<OsuHitObject>
{
    protected OsuHitObjectState(OsuHitObject hitObject, Beatmap beatmap)
        : base(hitObject, beatmap)
    {
    }
}

public abstract class OsuHitObjectState<TObject> : OsuHitObjectState
    where TObject : OsuHitObject
{
    public new TObject HitObject => (TObject)base.HitObject;

    protected OsuHitObjectState(TObject hitObject, Beatmap beatmap)
        : base(hitObject, beatmap)
    {
    }
}
