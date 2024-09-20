// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine;

internal static class FastRandom
{
    private static int _state;

    static FastRandom()
    {
        _state = 1;
    }

    public static void Seed(int seed)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(seed);
        _state = seed;
    }

    public static int Next()
    {
        _state = 214013 * _state + 2531011;
        return _state >> 16 & 0x7FFF;
    }

    public static int Next(int max) => (int)(max * NextSingle());

    public static int Next(int min, int max) => (int)((max - min) * NextSingle()) + min;

    public static float NextSingle() => Next() / (float)short.MaxValue;

    public static float NextSingle(float max) => max * NextSingle();

    public static float NextSingle(float min, float max) => (max - min) * NextSingle() + min;

    public static float NextAngle() => NextSingle(-MathF.PI, MathF.PI);

    public static unsafe void NextUnitVector(Vector2* vector)
    {
        float angle = NextAngle();
        vector->X = MathF.Cos(angle);
        vector->Y = MathF.Sin(angle);
    }
}
