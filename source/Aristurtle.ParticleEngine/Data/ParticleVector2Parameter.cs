// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Aristurtle.ParticleEngine.Data;

public struct ParticleVector2Parameter : IEquatable<ParticleVector2Parameter>
{
    public Vector2 Constant;
    public Vector2 RandomMin;
    public Vector2 RandomMax;
    public ParticleValueKind Kind;

    public Vector2 Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (Kind == ParticleValueKind.Constant)
            {
                return Constant;
            }

            Vector2 v;
            v.X = FastRandom.NextSingle(RandomMin.X, RandomMax.X);
            v.Y = FastRandom.NextSingle(RandomMin.Y, RandomMax.Y);
            return v;
        }
    }

    public ParticleVector2Parameter(Vector2 value)
    {
        Kind = ParticleValueKind.Constant;
        Constant = value;
        RandomMin = default;
        RandomMax = default;
    }

    public ParticleVector2Parameter(Vector2 rangeStart, Vector2 rangeEnd)
    {
        Kind = ParticleValueKind.Random;
        Constant = default;
        RandomMin = rangeStart;
        RandomMax = rangeEnd;
    }

    public override readonly bool Equals([NotNullWhen(true)] object obj) => obj is ParticleVector2Parameter other && Equals(other);

    public readonly bool Equals(ParticleVector2Parameter other)
    {
        if (Kind == ParticleValueKind.Constant)
        {
            return Constant.Equals(other.Constant);
        }

        return RandomMin.Equals(other.RandomMin) && RandomMax.Equals(other.RandomMax);
    }

    public override readonly int GetHashCode()
    {
        if (Kind == ParticleValueKind.Constant)
        {
            return Constant.GetHashCode();
        }

        return HashCode.Combine(RandomMin, RandomMax);
    }

    public static bool operator ==(ParticleVector2Parameter a, ParticleVector2Parameter b) => a.Equals(b);

    public static bool operator !=(ParticleVector2Parameter a, ParticleVector2Parameter b) => !a.Equals(b);
}
