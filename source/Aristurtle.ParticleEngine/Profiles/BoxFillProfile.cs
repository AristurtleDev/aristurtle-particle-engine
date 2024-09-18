// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public sealed class BoxFillProfile : Profile
{
    public float Width;
    public float Height;

    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        offset->X = FastRandom.NextSingle(Width * -0.5f, Width * 0.5f);
        offset->Y = FastRandom.NextSingle(Height * -0.5f, Height * 0.5f);
        FastRandom.NextUnitVector(heading);
    }
}
