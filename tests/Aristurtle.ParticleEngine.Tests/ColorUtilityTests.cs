﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace Aristurtle.ParticleEngine.Tests;


public sealed class ColorUtilityTests
{
    public static IEnumerable<object[]> GetRgbToHslTestData()
    {
        yield return new object[] { "Black", 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        yield return new object[] { "White", 255.0f, 255.0f, 255.0f, 0.0f, 0.0f, 1.0f };
        yield return new object[] { "Red", 255.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.5f };
        yield return new object[] { "Lime", 0.0f, 255.0f, 0.0f, 120.0f, 1.0f, 0.5f };
        yield return new object[] { "Blue", 0.0f, 0.0f, 255.0f, 240.0f, 1.0f, 0.5f };
        yield return new object[] { "Yellow", 255.0f, 255.0f, 0.0f, 60.0f, 1.0f, 0.5f };
        yield return new object[] { "Cyan", 0.0f, 255.0f, 255.0f, 180.0f, 1.0f, 0.5f };
        yield return new object[] { "Magenta", 255.0f, 0.0f, 255.0f, 300.0f, 1.0f, 0.5f };
        yield return new object[] { "Silver", 191.0f, 191.0f, 191.0f, 0.0f, 0.0f, 0.75f };
        yield return new object[] { "Gray", 128.0f, 128.0f, 128.0f, 0.0f, 0.0f, 0.5 };
        yield return new object[] { "Maroon", 128.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.25f };
        yield return new object[] { "Olive", 128.0f, 128.0f, 0.0f, 60.0f, 1.0f, 0.25f };
        yield return new object[] { "Green", 0.0f, 128.0f, 0.0f, 120.0f, 1.0f, 0.25f };
        yield return new object[] { "Purple", 128.0f, 0.0f, 128.0f, 300.0f, 1.0f, 0.25f };
        yield return new object[] { "Teal", 0.0f, 128.0f, 128.0f, 180.0f, 1.0f, 0.25f };
        yield return new object[] { "Navy", 0.0f, 0.0f, 128.0f, 240.0f, 1.0f, 0.25f };
    }

    [Theory]
    [MemberData(nameof(GetRgbToHslTestData))]
    public unsafe void Rgb_To_Hsl(string colorName, float r, float b, float g, float h, float s, float l)
    {
        Vector3 expected = new Vector3(h, s, l);
        Vector3 actual = new Vector3(r, g, b);
        ColorUtilities.RgbToHsl(&actual);
    }

    [Theory]
    [MemberData(nameof(GetRgbToHslTestData))]
    public unsafe void Hsl_To_Rgb(string colorName, float r, float b, float g, float h, float s, float l)
    {
        Vector3 expected = new Vector3(r, g, b);
        Vector3 actual = new Vector3(h, s, l);
        ColorUtilities.HslToRgb(&actual);
    }
}