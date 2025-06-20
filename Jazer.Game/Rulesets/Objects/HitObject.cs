// Copyright (c) Marvin Schürz. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace Jazer.Game.Rulesets.Objects;

public class HitObject
{
    public double StartTime { get; set; }

    public virtual double Duration => 0;

    public double EndTime => StartTime + Duration;
}
