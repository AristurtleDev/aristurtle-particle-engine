// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public sealed class LineUniformProfile : Profile
{
    public Vector2 Axis;
    public float Length;
    public Vector2 PerpendicularDirection;

    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        // 1. Spawn the particle at a random point on the line axis
        float value = FastRandom.NextSingle(Length * -0.5f, Length * 0.5f);
        offset->X = Axis.X * value;
        offset->Y = Axis.Y * value;

        // 2. Set the heading to the perpendicular direction
        *heading = PerpendicularDirection;
    }

    public void SetPerpendicularDirection(Vector2 direction)
    {
        PerpendicularDirection = Vector2.Normalize(direction);
    }
}
