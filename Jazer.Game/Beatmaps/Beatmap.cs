using System.Collections.Generic;
using Jazer.Game.Rulesets.Objects;

namespace Jazer.Game.Beatmaps;

public class Beatmap
{
    public readonly BeatmapDifficulty Difficulty = new BeatmapDifficulty();

    public readonly List<HitObject> HitObjects = new List<HitObject>();
}
