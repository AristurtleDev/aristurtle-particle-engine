// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine;

public static class ColorUtilities
{
    public static void RgbToHsl(int r, int g, int b, out float h, out float s, out float l)
    {
        float rf = r / 255.0f;
        float gf = g / 255.0f;
        float bf = b / 255.0f;
        RgbToHsl(rf, gf, bf, out h, out s, out l);
    }

    public static void RgbToHsl(Vector3 rgb, out float h, out float s, out float l) => RgbToHsl(rgb.X, rgb.Y, rgb.Z, out h, out s, out l);

    public static void RgbToHsl(float r, float g, float b, out float h, out float s, out float l)
    {
        float max = Math.Max(r, Math.Max(g, b));
        float min = Math.Min(r, Math.Min(g, b));
        float delta = max - min;

        h = delta == 0 ? 0 :
            max == r ? 60 * ((g - b) / delta % 6) :
            max == g ? 60 * ((b - r) / delta + 2) :
                       60 * ((r - g) / delta + 4);

        h = NormalizeHue(h);
        l = (max + min) * 0.5f;
        s = delta == 0 ? 0 : delta / (1 - Math.Abs(2 * l - 1));
    }

    public static void HslToRgb(Vector3 hsl, out int r, out int g, out int b) => HslToRgb(hsl.X, hsl.Y, hsl.Z, out r, out g, out b);

    public static void HslToRgb(float h, float s, float l, out int r, out int g, out int b)
    {
        double chroma = l < 0.5 ? l * (1.0 + s) : l + s - l * s;
        double lightness = 2.0 * l - chroma;

        double normalizedHue = h / 360.0;

        double redComponent = ComputeRGBComponent(lightness, chroma, NormalizeValue(normalizedHue + 1.0 / 3.0));
        double greenComponent = ComputeRGBComponent(lightness, chroma, normalizedHue);
        double blueComponent = ComputeRGBComponent(lightness, chroma, NormalizeValue(normalizedHue - 1.0 / 3.0));

        r = (byte)(redComponent * 255.0);
        g = (byte)(greenComponent * 255.0);
        b = (byte)(blueComponent * 255.0);
    }

    public static float NormalizeHue(float hue)
    {
        if (hue < 0)
        {
            return hue + 360 * ((int)hue / 360 + 1);
        }

        return hue % 360;
    }

    private static double NormalizeValue(double value)
    {
        if (value < 0.0)
        {
            return value + 1.0;
        }

        if (value > 1.0)
        {
            return value - 1.0;
        }

        return value;
    }

    private static double ComputeRGBComponent(double lightness, double chroma, double hueComponent)
    {
        if (hueComponent < 1.0 / 6.0)
        {
            return lightness + (chroma - lightness) * 6.0 * hueComponent;
        }
        if (hueComponent < 0.5)
        {
            return chroma;
        }
        if (hueComponent < 2.0 / 3.0)
        {
            return lightness + (chroma - lightness) * 6.0 * (2.0 / 3.0 - hueComponent);
        }
        return lightness;
    }
}
