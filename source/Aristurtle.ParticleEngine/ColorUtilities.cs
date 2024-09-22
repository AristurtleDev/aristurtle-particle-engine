// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine;

public static class ColorUtilities
{
    public static unsafe (int r, int g, int b) HslToRgb(float* value)
    {
        return HslToRgb(value[0], value[1], value[2]);
    }

    public static (int r, int g, int b) HslToRgb(Vector3 value)
    {
        return HslToRgb(value.X, value.Y, value.Z);
    }

    public static (int r, int g, int b) HslToRgb(float h, float s, float l)
    {
        double C = (1 - Math.Abs(2 * l - 1)) * s;
        double X = C * (1 - Math.Abs((h / 60) % 2 - 1));
        double m = l - C / 2;

        double rPrime, gPrime, bPrime;

        if (h < 60)
        {
            (rPrime, gPrime, bPrime) = (C, X, 0);
        }
        else if (h < 120)
        {
            (rPrime, gPrime, bPrime) = (X, C, 0);
        }
        else if (h < 180)
        {
            (rPrime, gPrime, bPrime) = (0, C, X);
        }
        else if (h < 240)
        {
            (rPrime, gPrime, bPrime) = (0, X, C);
        }
        else if (h < 300)
        {
            (rPrime, gPrime, bPrime) = (X, 0, C);
        }
        else
        {
            (rPrime, gPrime, bPrime) = (C, 0, X);
        }

        int r = (int)Math.Round((rPrime + m) * 255);
        int g = (int)Math.Round((gPrime + m) * 255);
        int b = (int)Math.Round((bPrime + m) * 255);

        return (r, g, b);
    }

    public static unsafe (float h, float s, float l) RgbToHsl(float* value)
    {
        return RgbToHsl(value[0], value[1], value[2]);
    }

    public static (float h, float s, float l) RgbToHsl(Vector3 value)
    {
        return RgbToHsl(value.X, value.Y, value.Z);
    }

    public static (float h, float s, float l) RgbToHsl(float r, float g, float b)
    {
        if (r > 1.0f)
        {
            r /= 255.0f;
        }

        if (g > 1.0f)
        {
            g /= 255.0f;
        }

        if (b > 1.0f)
        {
            b /= 255.0f;
        }

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        float h, s, l;

        if (delta == 0)
        {
            h = 0;
        }
        else if (max == r)
        {
            h = (float)(((g - b) / delta) % 6);
        }
        else if (max == g)
        {
            h = (float)((b - r) / delta + 2);
        }
        else
        {
            h = (float)((r - g) / delta + 4);
        }

        h *= 60;

        if (h < 0)
        {
            h += 360.0f;
        }

        l = (float)((max + min) / 2);

        if (delta == 0)
        {
            s = 0;
        }
        else
        {
            s = (float)(delta / (1 - Math.Abs(2 * l - 1)));
        }

        return (h, s, l);
    }
}
