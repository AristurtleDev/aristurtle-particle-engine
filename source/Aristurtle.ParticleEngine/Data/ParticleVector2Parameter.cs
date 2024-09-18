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
    public Vector2 RangeStart;
    public Vector2 RangeEnd;
    public ParticleValueKind Kind;

    public Vector2 Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (Kind == ParticleValueKind.Static)
            {
                return Constant;
            }

            Vector2 v;
            v.X = FastRandom.NextSingle(RangeStart.X, RangeEnd.X);
            v.Y = FastRandom.NextSingle(RangeStart.Y, RangeEnd.Y);
            return v;
        }
    }

    public ParticleVector2Parameter(Vector2 value)
    {
        Kind = ParticleValueKind.Static;
        Constant = value;
        RangeStart = default;
        RangeEnd = default;
    }

    public ParticleVector2Parameter(Vector2 rangeStart, Vector2 rangeEnd)
    {
        Kind = ParticleValueKind.Random;
        Constant = default;
        RangeStart = rangeStart;
        RangeEnd = rangeEnd;
    }

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is ParticleVector2Parameter other && Equals(other);

    public readonly bool Equals(ParticleVector2Parameter other)
    {
        if (Kind == ParticleValueKind.Static)
        {
            return Constant.Equals(other.Constant);
        }

        return RangeStart.Equals(other.RangeStart) && RangeEnd.Equals(other.RangeEnd);
    }

    public override readonly int GetHashCode()
    {
        if (Kind == ParticleValueKind.Static)
        {
            return Constant.GetHashCode();
        }

        return HashCode.Combine(RangeStart, RangeEnd);
    }

    public static bool operator ==(ParticleVector2Parameter a, ParticleVector2Parameter b) => a.Equals(b);

    public static bool operator !=(ParticleVector2Parameter a, ParticleVector2Parameter b) => !a.Equals(b);
}
