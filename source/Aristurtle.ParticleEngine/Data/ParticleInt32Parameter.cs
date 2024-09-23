// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Data;

public struct ParticleInt32Parameter : IEquatable<ParticleInt32Parameter>
{
    public int Constant;
    public int RandomMin;
    public int RandomMax;
    public ParticleValueKind Kind;

    [JsonIgnore]
    public int Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (Kind == ParticleValueKind.Constant)
            {
                return Constant;
            }

            return FastRandom.Next(RandomMin, RandomMax);
        }
    }

    public ParticleInt32Parameter(int value)
    {
        Kind = ParticleValueKind.Constant;
        Constant = value;
        RandomMin = default;
        RandomMax = default;
    }

    public ParticleInt32Parameter(int rangeStart, int rangeEnd)
    {
        Kind = ParticleValueKind.Random;
        Constant = default;
        RandomMin = rangeStart;
        RandomMax = rangeEnd;
    }

    public override readonly bool Equals([NotNullWhen(true)] object obj) => obj is ParticleInt32Parameter other && Equals(other);

    public readonly bool Equals(ParticleInt32Parameter other)
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

    public override readonly string ToString()
    {
        if (Kind == ParticleValueKind.Constant)
        {
            return Constant.ToString(CultureInfo.InvariantCulture);
        }

        return string.Format(NumberFormatInfo.InvariantInfo, "{0}{1}{2}", RandomMin, NumberFormatInfo.InvariantInfo.NumberGroupSeparator, RandomMax);
    }

    public static bool operator ==(ParticleInt32Parameter a, ParticleInt32Parameter b) => a.Equals(b);

    public static bool operator !=(ParticleInt32Parameter a, ParticleInt32Parameter b) => !a.Equals(b);
}
