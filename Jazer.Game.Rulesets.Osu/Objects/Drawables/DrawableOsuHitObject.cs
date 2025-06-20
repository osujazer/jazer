using Jazer.Game.Rulesets.Objects.Drawables;

namespace Jazer.Game.Rulesets.Osu.Objects.Drawables;

public partial class DrawableOsuHitObject<T> : DrawableHitObject<T>
    where T : OsuHitObject
{
    public DrawableOsuHitObject(T hitObject)
        : base(hitObject)
    {
    }
}
