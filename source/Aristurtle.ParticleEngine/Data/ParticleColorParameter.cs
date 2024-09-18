// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Aristurtle.ParticleEngine.Data;

public struct ParticleColorParameter : IEquatable<ParticleColorParameter>
{
    public Vector3 Static;
    public Vector3 RandomMin;
    public Vector3 RandomMax;
    public ParticleValueKind Kind;

    public Vector3 Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (Kind == ParticleValueKind.Static)
            {
                return Static;
            }
            else
            {
                Vector3 hsl;
                hsl.X = FastRandom.NextSingle(RandomMin.X, RandomMax.X);
                hsl.Y = FastRandom.NextSingle(RandomMin.Y, RandomMax.Y);
                hsl.Z = FastRandom.NextSingle(RandomMin.Z, RandomMax.Z);
                return hsl;
            }
        }
    }

    public ParticleColorParameter(Vector3 value)
    {
        Kind = ParticleValueKind.Static;

        Static = value;

        RandomMin = default;
        RandomMax = default;
    }

    public ParticleColorParameter(Vector3 rangeStart, Vector3 rangeEnd)
    {
        Kind = ParticleValueKind.Random;
        Static = default;

        RandomMin = rangeStart;
        RandomMax = rangeEnd;
    }

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is ParticleColorParameter other && Equals(other);

    public readonly bool Equals(ParticleColorParameter other)
    {
        if (Kind == ParticleValueKind.Static)
        {
            return Static.Equals(other.Static);
        }

        return RandomMin.Equals(other.RandomMin) && RandomMax.Equals(other.RandomMax);
    }

    public override readonly int GetHashCode()
    {
        if (Kind == ParticleValueKind.Static)
        {
            return Kind.GetHashCode();
        }

        return HashCode.Combine(RandomMin, RandomMax);
    }

    public static bool operator ==(ParticleColorParameter a, ParticleColorParameter b) => a.Equals(b);

    public static bool operator !=(ParticleColorParameter a, ParticleColorParameter b) => !a.Equals(b);
}
