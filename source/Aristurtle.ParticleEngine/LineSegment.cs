// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Aristurtle.ParticleEngine;

[StructLayout(LayoutKind.Sequential)]
public readonly struct LineSegment : IEquatable<LineSegment>
{
    internal readonly Vector2 _point1;
    internal readonly Vector2 _point2;

    public readonly Vector2 Origin => _point1;
    public readonly Vector2 Direction => _point2 - _point1;

    public LineSegment(Vector2 point1, Vector2 point2) => (_point1, _point2) = (point1, point2);

    public LineSegment Translate(Vector2 vector) => new LineSegment(_point1 + vector, _point2 + vector);

    public Vector2 ToVector2() => _point2 - _point1;

    public static LineSegment FromPoints(Vector2 point1, Vector2 point2) => new LineSegment(point1, point2);

    public static LineSegment FromOrigin(Vector2 origin, Vector2 vector) => new LineSegment(origin, origin + vector);

    public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is LineSegment other && Equals(other);

    public readonly bool Equals(LineSegment other) => _point1.Equals(other._point1) &&
                                                      _point2.Equals(other._point2);
    public override readonly int GetHashCode() => HashCode.Combine(_point1, _point2);

    public override readonly string ToString() => $"({_point1:x}:{_point1:y},{_point2:x}:{_point2:y})";

    public static bool operator ==(LineSegment a, LineSegment b) => a.Equals(b);

    public static bool operator !=(LineSegment a, LineSegment b) => !a.Equals(b);

}
