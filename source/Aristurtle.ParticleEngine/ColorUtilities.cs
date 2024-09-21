// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine;

public static class ColorUtilities
{
    public static unsafe void HslToRgb(float h, float s, float l, out int r, out int g, out int b)
    {
        Vector3 value = new Vector3(h, s, l);
        HslToRgb(&value);
        r = (int)value.X;
        g = (int)value.Y;
        b = (int)value.Z;
    }

    public static unsafe void HslToRgb(Vector3* value)
    {
        double C = (1 - Math.Abs(2 * value->Z - 1)) * value->Y;
        double X = C * (1 - Math.Abs((value->X / 60) % 2 - 1));
        double m = value->Z - C / 2;

        double rPrime, gPrime, bPrime;

        if (value->X < 60)
        {
            (rPrime, gPrime, bPrime) = (C, X, 0);
        }
        else if (value->X < 120)
        {
            (rPrime, gPrime, bPrime) = (X, C, 0);
        }
        else if (value->X < 180)
        {
            (rPrime, gPrime, bPrime) = (0, C, X);
        }
        else if (value->X < 240)
        {
            (rPrime, gPrime, bPrime) = (0, X, C);
        }
        else if (value->X < 300)
        {
            (rPrime, gPrime, bPrime) = (X, 0, C);
        }
        else
        {
            (rPrime, gPrime, bPrime) = (C, 0, X);
        }

        value->X = (int)Math.Round((rPrime + m) * 255);
        value->Y = (int)Math.Round((gPrime + m) * 255);
        value->Z = (int)Math.Round((bPrime + m) * 255);
    }

    public static unsafe void RgbToHsl(float r, float g, float b, out float h, out float s, out float l)
    {
        Vector3 value = new Vector3(r, g, b);
        RgbToHsl(&value);
        h = value.X;
        s = value.Y;
        l = value.Z;
    }

    public static unsafe void RgbToHsl(Vector3* value)
    {
        if (value->X > 1.0f)
        {
            value->X /= 255.0f;
        }

        if (value->Y > 1.0f)
        {
            value->Y /= 255.0f;
        }

        if (value->Z > 1.0f)
        {
            value->Z /= 255.0f;
        }

        double max = Math.Max(value->X, Math.Max(value->Y, value->Z));
        double min = Math.Min(value->X, Math.Min(value->Y, value->Z));
        double delta = max - min;

        if (delta == 0)
        {
            value->X = 0;
        }
        else if (max == value->X)
        {
            value->X = (float)(((value->Y - value->Z) / delta) % 6);
        }
        else if (max == value->Y)
        {
            value->X = (float)((value->Z - value->X) / delta + 2);
        }
        else
        {
            value->X = (float)((value->X - value->Y) / delta + 4);
        }

        value->X *= 60;

        if (value->X < 0)
        {
            value->X += 360.0f;
        }

        value->Z = (float)((max + min) / 2);

        if (delta == 0)
        {
            value->Y = 0;
        }
        else
        {
            value->Y = (float)(delta / (1 - Math.Abs(2 * value->Z - 1)));
        }
    }
}
