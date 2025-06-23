namespace Jazer.Game.Beatmaps;

public class BeatmapInfo
{
    public BeatmapMetadata Metadata = new BeatmapMetadata();

    public BeatmapSetInfo BeatmapSet { get; set; } = null!;
}
