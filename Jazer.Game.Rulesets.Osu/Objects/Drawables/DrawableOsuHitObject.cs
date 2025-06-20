// Copyright (c) Marvin Schürz. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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
