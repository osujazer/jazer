using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace Jazer.Game.Graphics;

public partial class JazerSpriteText : SpriteText
{
    public JazerSpriteText()
    {
        Font = JazerFont.Default;
        base.Padding = new MarginPadding { Vertical = -3 };
    }

    public new MarginPadding Padding
    {
        get => base.Padding with { Top = base.Padding.Top + 3, Bottom = base.Padding.Bottom + 3, };
        set => base.Padding = value with { Top = value.Top - 3, Bottom = value.Bottom - 3, };
    }
}
