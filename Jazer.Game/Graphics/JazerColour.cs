using System;
using osuTK.Graphics;

namespace Jazer.Game.Graphics;

public class JazerColour
{
    public static Color4 Gray(float brightness) => new Color4(brightness, brightness, MathF.Pow(brightness, 0.92f), 1f);

    public static Color4 Gray100 => Gray(0.9f);
    public static Color4 Gray200 => Gray(0.7f);
    public static Color4 Gray300 => Gray(0.5f);
    public static Color4 Gray400 => Gray(0.35f);
    public static Color4 Gray500 => Gray(0.22f);
    public static Color4 Gray600 => Gray(0.15f);
    public static Color4 Gray700 => Gray(0.1f);
    public static Color4 Gray800 => Gray(0.05f);
    public static Color4 Gray900 => Gray(0.025f);
}
