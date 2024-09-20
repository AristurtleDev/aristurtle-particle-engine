// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aristurtle.ParticleEngine;

[StructLayout(LayoutKind.Explicit)]
public struct ParticleColor
{
    [FieldOffset(0)]
    public float Hue;

    [FieldOffset(4)]
    public float Saturation;

    [FieldOffset(8)]
    public float Lightness;

    public void ToRgb(out int r, out int g, out int b)
    {
        double C = (1 - Math.Abs(2 * Lightness - 1)) * Saturation;
        double X = C * (1 - Math.Abs((Hue / 60) % 2 - 1));
        double m = Lightness - C / 2;

        double rPrime, gPrime, bPrime;

        if (Hue < 60) (rPrime, gPrime, bPrime) = (C, X, 0);
        else if (Hue < 120) (rPrime, gPrime, bPrime) = (X, C, 0);
        else if (Hue < 180) (rPrime, gPrime, bPrime) = (0, C, X);
        else if (Hue < 240) (rPrime, gPrime, bPrime) = (0, X, C);
        else if (Hue < 300) (rPrime, gPrime, bPrime) = (X, 0, C);
        else (rPrime, gPrime, bPrime) = (C, 0, X);

        r = (int)Math.Round((rPrime + m) * 255);
        g = (int)Math.Round((gPrime + m) * 255);
        b = (int)Math.Round((bPrime + m) * 255);
    }
}
