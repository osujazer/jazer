using System;
using System.Linq;
using Jazer.Game.Beatmaps;
using Jazer.Game.Graphics.Carousel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Pooling;
using osu.Framework.Logging;

namespace Jazer.Game.Screens.Select;

[Cached]
public partial class BeatmapCarousel : Carousel<BeatmapInfo>
{
    private DrawablePool<PanelBeatmapSet> mapsetPanels = null!;
    private DrawablePool<PanelBeatmap> beatmapPanels = null!;

    private readonly BeatmapCarouselFilterGrouping grouping;

    public BeatmapCarousel()
    {
        DebounceDelay = 100;
        DistanceOffscreenToPreload = 100;

        Filters =
        [
            grouping = new BeatmapCarouselFilterGrouping()
        ];
    }

    [BackgroundDependencyLoader]
    private void load()
    {
        AddRangeInternal([
            mapsetPanels = new DrawablePool<PanelBeatmapSet>(10, 40),
            beatmapPanels = new DrawablePool<PanelBeatmap>(10, 40),
        ]);
    }

    protected override Drawable GetDrawableForDisplay(CarouselItem item) => item.Model switch
    {
        BeatmapSetInfo => mapsetPanels.Get(),
        BeatmapInfo => beatmapPanels.Get(),
        _ => Empty()
    };

    protected override float GetSpacingBetweenPanels(CarouselItem top, CarouselItem bottom) => -3;

    protected override void HandleItemActivated(CarouselItem item)
    {
        switch (item.Model)
        {
            case BeatmapSetInfo setInfo:
                if (grouping.SetItems.TryGetValue(setInfo, out var items))
                {
                    var beatmaps = items.Select(i => i.Model).OfType<BeatmapInfo>();
                    CurrentSelection = beatmaps.FirstOrDefault();
                }

                break;
        }
    }

    protected override void HandleItemSelected(object? model)
    {
        base.HandleItemSelected(model);

        Logger.Log($"Selected item {model}");

        switch (model)
        {
            case BeatmapSetInfo:
                throw new InvalidOperationException("Groups should never become selected");

            case BeatmapInfo beatmapInfo:
                setExpandedSet(beatmapInfo);
                break;
        }
    }

    protected BeatmapSetInfo? ExpandedBeatmapSet { get; private set; }

    private void setExpandedSet(BeatmapInfo beatmapInfo)
    {
        if (ExpandedBeatmapSet != null)
            setExpansionStateOfSetItems(ExpandedBeatmapSet, false);
        ExpandedBeatmapSet = beatmapInfo.BeatmapSet!;
        setExpansionStateOfSetItems(ExpandedBeatmapSet, true);
    }

    private void setExpansionStateOfSetItems(BeatmapSetInfo set, bool expanded)
    {
        if (grouping.SetItems.TryGetValue(set, out var items))
        {
            foreach (var i in items)
            {
                if (i.Model is BeatmapSetInfo)
                    i.IsExpanded = expanded;
                else
                    i.IsVisible = expanded;
            }
        }
    }
}
