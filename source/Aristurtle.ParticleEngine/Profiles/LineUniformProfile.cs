// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public sealed class LineProfile : Profile
{
    public Vector2 Axis;
    public float Length;

    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        float value = FastRandom.NextSingle(Length * -0.5f, Length * 0.5f);
        offset->X = Axis.X * value;
        offset->Y = Axis.Y * value;
        FastRandom.NextUnitVector(heading);
    }
}
