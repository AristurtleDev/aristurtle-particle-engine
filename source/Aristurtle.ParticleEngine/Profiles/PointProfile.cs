// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public sealed class PointProfile : Profile
{
    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        offset->X = 0.0f;
        offset->Y = 0.0f;
        FastRandom.NextUnitVector(heading);
    }
}
