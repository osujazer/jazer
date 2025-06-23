using Jazer.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK.Graphics;

namespace Jazer.Game.Screens.Select;

public partial class PanelBeatmapSet : Panel
{
    public const float HEIGHT = 55;

    [BackgroundDependencyLoader]
    private void load()
    {
        Background = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Masking = true,
            BorderColour = Color4.Black.Opacity(0.75f),
            BorderThickness = 2,
            Alpha = 0.85f,
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Color4.Orange,
            }
        };

        Content.Add(new GridContainer
        {
            RelativeSizeAxes = Axes.Both,
            ColumnDimensions =
            [
                new Dimension(GridSizeMode.Absolute, 80f),
                new Dimension()
            ],
            RowDimensions = [new Dimension()],
            Content = new[]
            {
                new Drawable[]
                {
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = new Box
                        {
                            Colour = Color4.Gray,
                            RelativeSizeAxes = Axes.Both,
                        }
                    },
                    new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Padding = new MarginPadding { Horizontal = 10, Vertical = 3 },
                        Children =
                        [
                            new JazerSpriteText
                            {
                                Text = "Beatmap title",
                                Font = JazerFont.Get(size: 22f),
                                Alpha = 0.75f,
                            },
                            new JazerSpriteText
                            {
                                Text = "Beatmap artist",
                                Font = JazerFont.Get(size: 18f),
                                Alpha = 0.75f,
                            },
                        ]
                    }
                }
            }
        });
    }
}
