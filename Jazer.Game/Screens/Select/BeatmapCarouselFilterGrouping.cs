using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Jazer.Game.Beatmaps;
using Jazer.Game.Graphics.Carousel;

namespace Jazer.Game.Screens.Select;

public class BeatmapCarouselFilterGrouping : ICarouselFilter
{
    private readonly Dictionary<BeatmapSetInfo, HashSet<CarouselItem>> setMap = new Dictionary<BeatmapSetInfo, HashSet<CarouselItem>>();

    public IReadOnlyDictionary<BeatmapSetInfo, HashSet<CarouselItem>> SetItems => setMap;

    public async Task<List<CarouselItem>> Run(IEnumerable<CarouselItem> items, CancellationToken cancellationToken)
    {
        BeatmapInfo? lastBeatmap = null;

        return await Task.Run(() =>
        {
            setMap.Clear();

            var newItems = new List<CarouselItem>();

            HashSet<CarouselItem>? currentSetItems = null;

            foreach (var item in items)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var beatmap = (BeatmapInfo)item.Model;

                if (lastBeatmap?.BeatmapSet != beatmap.BeatmapSet)
                {
                    if (!setMap.TryGetValue(beatmap.BeatmapSet!, out currentSetItems))
                        setMap[beatmap.BeatmapSet!] = currentSetItems = new HashSet<CarouselItem>();

                    addItem(new CarouselItem(beatmap.BeatmapSet!)
                    {
                        DrawHeight = PanelBeatmapSet.HEIGHT,
                        DepthLayer = -1
                    });
                }

                addItem(item);
                lastBeatmap = beatmap;
            }

            void addItem(CarouselItem i)
            {
                newItems.Add(i);

                currentSetItems?.Add(i);

                i.IsVisible = (i.Model is BeatmapSetInfo || currentSetItems == null);
            }

            return newItems;
        }, cancellationToken).ConfigureAwait(false);
    }
}
