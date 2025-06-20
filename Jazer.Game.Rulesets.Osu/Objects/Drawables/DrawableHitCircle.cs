// Copyright (c) Marvin Schürz. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace Jazer.Game.Rulesets.Osu.Objects.Drawables;

public partial class DrawableHitCircle : DrawableOsuHitObject<HitCircle>
{
    public DrawableHitCircle(HitCircle hitObject)
        : base(hitObject)
    {
    }

    private Circle circlePiece;
    private CircularContainer approachCircle;

    [BackgroundDependencyLoader]
    private void load()
    {
        Position = HitObject.Position;
        Origin = Anchor.Centre;
        Size = new Vector2(64);

        AddRangeInternal([
            circlePiece = new Circle
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            },
            approachCircle = new CircularContainer
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                Masking = true,
                BorderColour = Color4.White,
                BorderThickness = 4,
                Scale = new Vector2(4),
                Alpha = 0,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    AlwaysPresent = true
                }
            }
        ]);

        using (BeginAbsoluteSequence(HitObject.StartTime - 1000))
        {
            circlePiece.FadeIn(700);
            approachCircle
                .FadeIn(1000)
                .ScaleTo(1, 1000);
        }

        using (BeginAbsoluteSequence(HitObject.StartTime))
        {
            approachCircle.FadeOut();

            circlePiece.ScaleTo(1.5f, 200)
                       .FadeOut(200);
        }
    }
}
