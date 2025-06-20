// Copyright (c) Marvin Schürz. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using Jazer.Game.Rulesets.Objects;

namespace Jazer.Game.Beatmaps;

public class Beatmap
{
    public readonly List<HitObject> HitObjects = new List<HitObject>();
}
