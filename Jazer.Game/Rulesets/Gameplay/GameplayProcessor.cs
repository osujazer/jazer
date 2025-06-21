using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Objects;
using osu.Framework.Graphics.Performance;
using osu.Framework.Timing;

namespace Jazer.Game.Rulesets.Gameplay;

public abstract class GameplayProcessor
{
    public abstract event Action<HitObjectState>? HitObjectBecameAlive;
    public abstract event Action<HitObjectState>? HitObjectBecameDead;

    protected Beatmap Beatmap { get; }

    public abstract IReadOnlyList<HitObjectState> HitObjects { get; }

    public abstract IReadOnlyCollection<HitObjectState> AliveObjects { get; }

    protected GameplayProcessor(Beatmap beatmap)
    {
        Beatmap = beatmap;
    }

    public abstract void Update(double currentTime /* TODO: IEnumerable<Something> inputEvents */);
}

public abstract class GameplayProcessor<TObject> : GameplayProcessor
    where TObject : HitObjectState
{
    public override event Action<HitObjectState>? HitObjectBecameAlive;

    public override event Action<HitObjectState>? HitObjectBecameDead;

    private readonly List<TObject> hitObjects;
    private readonly LifetimeEntryManager lifetimeManager = new LifetimeEntryManager();
    private readonly ManualClock clock;
    private readonly IFrameBasedClock framedClock;
    private readonly SortedSet<TObject> aliveObjects = new SortedSet<TObject>(new HitObjectState.Comparer());

    protected FrameTimeInfo Time => framedClock.TimeInfo;

    public override IReadOnlyList<TObject> HitObjects => hitObjects;

    public override IReadOnlyCollection<TObject> AliveObjects => aliveObjects;

    protected GameplayProcessor(Beatmap beatmap)
        : base(beatmap)
    {
        clock = new ManualClock();
        framedClock = new FramedClock(clock);

        hitObjects = beatmap.HitObjects.Select(createHitObjectState).ToList();
        hitObjects.ForEach(lifetimeManager.AddEntry);

        lifetimeManager.EntryBecameAlive += onEntryBecameAlive;
        lifetimeManager.EntryBecameDead += onEntryBecameDead;
    }

    [Pure]
    protected abstract TObject CreateHitObjectState(HitObject hitObject, Beatmap beatmap);

    protected abstract void ProcessFrame();

    public sealed override void Update(double currentTime)
    {
        clock.CurrentTime = currentTime;
        framedClock.ProcessFrame();

        lifetimeManager.Update(currentTime);

        ProcessFrame();
    }

    private TObject createHitObjectState(HitObject hitObject)
    {
        var obj = CreateHitObjectState(hitObject, Beatmap);

        obj.ApplyDefaults();

        return obj;
    }

    private void onEntryBecameAlive(LifetimeEntry entry)
    {
        var obj = (TObject)entry;

        aliveObjects.Add(obj);

        OnHitObjectBecameAlive(obj);
        HitObjectBecameAlive?.Invoke(obj);
    }

    private void onEntryBecameDead(LifetimeEntry entry)
    {
        var obj = (TObject)entry;

        aliveObjects.Remove(obj);

        OnHitObjectBecameDead((TObject)entry);

        HitObjectBecameDead?.Invoke((TObject)entry);
    }

    protected virtual void OnHitObjectBecameAlive(TObject hitObject)
    {
    }

    protected virtual void OnHitObjectBecameDead(TObject hitObject)
    {
    }
}
