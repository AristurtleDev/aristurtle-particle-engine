// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public abstract class Profile
{
    public abstract unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading);

    public static Profile Point()
    {
        return new PointProfile();
    }

    public static Profile Line(Vector2 axis, float length)
    {
        return new LineProfile { Axis = axis, Length = length };
    }

    public static Profile Ring(float radius, CircleRadiation radiate)
    {
        return new RingProfile { Radius = radius, Radiate = radiate };
    }

    public static Profile Box(float width, float height)
    {
        return new BoxProfile { Width = width, Height = height };
    }

    public static Profile BoxFill(float width, float height)
    {
        return new BoxFillProfile { Width = width, Height = height };
    }

    public static Profile BoxUniform(float width, float height)
    {
        return new BoxUniformProfile { Width = width, Height = height };
    }

    public static Profile Circle(float radius, CircleRadiation radiate)
    {
        return new CircleProfile { Radius = radius, Radiate = radiate };
    }

    public static Profile Spray(Vector2 direction, float spread)
    {
        return new SprayProfile { Direction = direction, Spread = spread };
    }

    public override string ToString()
    {
        return GetType().ToString();
    }
}
