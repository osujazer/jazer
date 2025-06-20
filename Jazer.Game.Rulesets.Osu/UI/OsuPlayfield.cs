// Copyright (c) Marvin Schürz. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Jazer.Game.Beatmaps;
using Jazer.Game.Rulesets.Objects;
using Jazer.Game.Rulesets.Objects.Drawables;
using Jazer.Game.Rulesets.Osu.Objects;
using Jazer.Game.Rulesets.Osu.Objects.Drawables;
using Jazer.Game.Rulesets.UI;

namespace Jazer.Game.Rulesets.Osu.UI;

public partial class OsuPlayfield : Playfield
{
    public OsuPlayfield(Beatmap beatmap)
        : base(beatmap)
    {
    }

    protected override DrawableHitObject CreateDrawableFor(HitObject hitObject) => hitObject switch
    {
        HitCircle hitCircle => new DrawableHitCircle(hitCircle),
        _ => throw new ArgumentOutOfRangeException(nameof(hitObject), hitObject, null)
    };
}
