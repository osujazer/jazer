using System.Collections.Generic;
using System.Linq;

namespace Jazer.Game.Beatmaps;

public class BeatmapSetInfo
{
    private readonly List<BeatmapInfo> beatmaps = new List<BeatmapInfo>();

    public IReadOnlyList<BeatmapInfo> Beatmaps
    {
        get => beatmaps;
        set
        {
            beatmaps.Clear();
            beatmaps.AddRange(value);
            foreach (var b in beatmaps)
                b.BeatmapSet = this;
        }
    }

    public BeatmapMetadata Metadata => Beatmaps.FirstOrDefault()?.Metadata ?? new BeatmapMetadata();
}
