using System.Collections.Generic;
using Jazer.Game.Rulesets.Gameplay;
using Jazer.Game.Rulesets.Objects;
using Jazer.Game.Rulesets.Objects.Drawables;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace Jazer.Game.Rulesets.UI;

public abstract partial class Playfield : CompositeDrawable
{
    protected readonly GameplayProcessor GameplayProcessor;

    private readonly Dictionary<HitObjectState, DrawableHitObject> hitObjectMap = new Dictionary<HitObjectState, DrawableHitObject>();

    private Container<DrawableHitObject> hitObjectContainer = null!;

    public IReadOnlyList<DrawableHitObject> HitObjects => hitObjectContainer.Children;

    protected Playfield(GameplayProcessor gameplayProcessor)
    {
        this.GameplayProcessor = gameplayProcessor;
    }

    protected abstract DrawableHitObject CreateDrawableFor(HitObject hitObject);

    [BackgroundDependencyLoader]
    private void load()
    {
        AddInternal(hitObjectContainer = new Container<DrawableHitObject>
        {
            RelativeSizeAxes = Axes.Both,
        });
    }

    protected override void LoadComplete()
    {
        base.LoadComplete();

        GameplayProcessor.HitObjectBecameAlive += addDrawableHitObject;
        GameplayProcessor.HitObjectBecameDead += removeDrawableHitObject;
    }

    private void addDrawableHitObject(HitObjectState state)
    {
        var dho = CreateDrawableFor(state.HitObject);

        hitObjectMap[state] = dho;

        hitObjectContainer.Add(dho);
    }

    private void removeDrawableHitObject(HitObjectState state)
    {
        if (!hitObjectMap.Remove(state, out var dho))
            return;

        hitObjectContainer.Remove(dho, true);
    }

    protected override void Dispose(bool isDisposing)
    {
        base.Dispose(isDisposing);

        GameplayProcessor.HitObjectBecameAlive += addDrawableHitObject;
        GameplayProcessor.HitObjectBecameDead += removeDrawableHitObject;
    }
}
