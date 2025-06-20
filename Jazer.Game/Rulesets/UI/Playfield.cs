// Copyright (c) Marvin Schürz. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Objects;
using Jazer.Game.Rulesets.Objects.Drawables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Performance;

namespace Jazer.Game.Rulesets.UI;

public abstract partial class Playfield : LifetimeManagementContainer
{
    protected Beatmap Beatmap { get; private set; }

    private readonly LifetimeEntryManager lifetimeManager = new LifetimeEntryManager();
    private readonly Dictionary<HitObjectLifetimeEntry, DrawableHitObject> hitObjectMap = new Dictionary<HitObjectLifetimeEntry, DrawableHitObject>();

    protected Playfield(Beatmap beatmap)
    {
        Beatmap = beatmap;

        lifetimeManager.EntryBecameAlive += entry => addDrawableHitObject((HitObjectLifetimeEntry)entry);
        lifetimeManager.EntryBecameDead += entry => removeDrawableHitObject((HitObjectLifetimeEntry)entry);
    }

    protected abstract DrawableHitObject CreateDrawableFor(HitObject hitObject);

    protected virtual HitObjectLifetimeEntry CreateLifetimeEntry(HitObject hitObject) => new HitObjectLifetimeEntry(hitObject);

    protected override void LoadComplete()
    {
        base.LoadComplete();

        foreach (var hitObject in Beatmap.HitObjects)
            addHitObject(hitObject);
    }

    private void addHitObject(HitObject hitObject)
    {
        var entry = new HitObjectLifetimeEntry(hitObject);

        lifetimeManager.AddEntry(entry);
    }

    private void addDrawableHitObject(HitObjectLifetimeEntry entry)
    {
        var dho = CreateDrawableFor(entry.HitObject);

        hitObjectMap[entry] = dho;

        AddInternal(dho);
    }

    private void removeDrawableHitObject(HitObjectLifetimeEntry entry)
    {
        if (!hitObjectMap.Remove(entry, out var dho))
            return;

        RemoveInternal(dho, true);
    }

    protected override bool UpdateChildrenLife()
    {
        if (!IsPresent)
            return false;

        bool aliveChanged = base.CheckChildrenLife();
        aliveChanged |= lifetimeManager.Update(Time.Current);
        return aliveChanged;
    }

    public class HitObjectLifetimeEntry : LifetimeEntry
    {
        public readonly HitObject HitObject;

        public HitObjectLifetimeEntry(HitObject hitObject)
        {
            HitObject = hitObject;

            LifetimeStart = hitObject.StartTime - PastLifetimeExtension;
            LifetimeEnd = hitObject.EndTime + FutureLifetimeExtension;
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
}
