using osu.Framework.Graphics.Sprites;

namespace Jazer.Game.Graphics;

public class JazerFont
{
    public static FontUsage Default => Get();

    public static FontUsage Get(float size = 14, FontWeight weight = FontWeight.Regular) =>
        new FontUsage(family: "NunitoSans", size: size, weight: weight.ToString());
}

public enum FontWeight
{
    Regular,
    Medium,
    SemiBold,
    Bold,
}
