// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public sealed class BoxProfile : Profile
{
    public float Width;
    public float Height;

    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        switch (FastRandom.Next(4))
        {
            case 0: // Left
                offset->X = Width * -0.5f;
                offset->Y = FastRandom.NextSingle(Height * -0.5f, Height * 0.5f);
                break;

            case 1: // Top
                offset->X = FastRandom.NextSingle(Width * -0.5f, Width * 0.5f);
                offset->Y = Height * -0.5f;
                break;

            case 2: // Right
                offset->X = Width * 0.5f;
                offset->Y = FastRandom.NextSingle(Height * -0.5f, Height * 0.5f);
                break;

            default: // Bottom
                offset->X = FastRandom.NextSingle(Width * -0.5f, Width * 0.5f);
                offset->Y = Height * 0.5f;
                break;
        }

        FastRandom.NextUnitVector(heading);
    }
}
