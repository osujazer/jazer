using System.Collections.Generic;
using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Objects;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Performance;

namespace Jazer.Game.Rulesets.Gameplay;

public abstract class HitObjectState : LifetimeEntry
{
    public class Comparer : Comparer<HitObjectState>
    {
        public override int Compare(HitObjectState? x, HitObjectState? y) =>
            x?.HitObject.StartTime.CompareTo(y?.HitObject.StartTime) ?? 0;
    }

    public readonly HitObject HitObject;

    public readonly Beatmap Beatmap;

    public readonly Bindable<ArmedState> State = new Bindable<ArmedState>();

    protected HitObjectState(HitObject hitObject, Beatmap beatmap)
    {
        HitObject = hitObject;
        Beatmap = beatmap;

        updateLifetime();
    }

    private void updateLifetime()
    {
        LifetimeStart = HitObject.StartTime - PastLifetimeExtension;
        LifetimeEnd = HitObject.EndTime + FutureLifetimeExtension;
    }

    public virtual void ApplyDefaults()
    {
    }

    /// <summary>
    /// The amount of time that a hitObject will become alive before its start time
    /// </summary>
    protected virtual double PastLifetimeExtension => 1000;

    /// <summary>
    /// The amount of time that a hitObject will remain alive after it's end time
    /// </summary>
    protected virtual double FutureLifetimeExtension => 1000;
}

public abstract class HitObjectState<TObject> : HitObjectState
    where TObject : HitObject
{
    public new TObject HitObject => (TObject)base.HitObject;

    protected HitObjectState(TObject hitObject, Beatmap beatmap)
        : base(hitObject, beatmap)
    {
    }
}
