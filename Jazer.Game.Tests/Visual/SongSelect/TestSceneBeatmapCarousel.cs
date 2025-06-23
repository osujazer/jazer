using System.Collections.Generic;
using System.Linq;
using Jazer.Game.Beatmaps;
using Jazer.Game.Screens.Select;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace Jazer.Game.Tests.Visual.SongSelect;

public partial class TestSceneBeatmapCarousel : JazerTestScene
{
    private TestCarousel carousel = null!;

    [BackgroundDependencyLoader]
    private void load()
    {
        Add(carousel = new TestCarousel
        {
            Anchor = Anchor.TopRight,
            Origin = Anchor.TopRight,
            Width = 500,
            RelativeSizeAxes = Axes.Y,
        });

        var items = new List<BeatmapSetInfo>();

        for (int i = 0; i < 1000; i++)
        {
            items.Add(new BeatmapSetInfo
            {
                Beatmaps =
                [
                    new BeatmapInfo(),
                    new BeatmapInfo(),
                    new BeatmapInfo(),
                ]
            });
        }

        carousel.Items.AddRange(items.SelectMany(s => s.Beatmaps));
    }

    private partial class TestCarousel : BeatmapCarousel
    {
        public new BindableList<BeatmapInfo> Items => (BindableList<BeatmapInfo>)base.Items;
    }
}
