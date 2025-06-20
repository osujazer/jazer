using osu.Framework.Graphics.Containers;

namespace Jazer.Game.Rulesets.Objects.Drawables;

public abstract partial class DrawableHitObject : CompositeDrawable
{
    public override bool RemoveWhenNotAlive => false;

    public override bool RemoveCompletedTransforms => false;

    public HitObject HitObject { get; private set; }

    protected DrawableHitObject(HitObject hitObject)
    {
        HitObject = hitObject;
    }
}

public abstract partial class DrawableHitObject<T> : DrawableHitObject
    where T : HitObject
{
    public new T HitObject => (T)base.HitObject;

    protected DrawableHitObject(T hitObject)
        : base(hitObject)
    {
    }
}
