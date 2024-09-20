// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public class BoxUniformProfile : Profile
{
    public float Width;
    public float Height;

    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        int perimeter = (int)(2 * Width + 2 * Height);
        int value = FastRandom.Next(perimeter);

        switch (value)
        {
            //  Top
            case var _ when value < Width:
                offset->X = FastRandom.NextSingle(Width * -0.5f, Width * 0.5f);
                offset->Y = Height * -0.5f;
                break;

            //  Bottom
            case var _ when value < 2 * Width:
                offset->X = FastRandom.NextSingle(Width * -0.5f, Width * 0.5f);
                offset->Y = Height * 0.5f;
                break;

            //  Left
            case var _ when value < 2 * Width + Height:
                offset->X = Width * -0.5f;
                offset->Y = FastRandom.NextSingle(Height * -0.5f, Height * 0.5f);
                break;

            // Right
            default:
                offset->X = Width * 0.5f;
                offset->Y = FastRandom.NextSingle(Height * -0.5f, Height * 0.5f);
                break;
        }

        FastRandom.NextUnitVector(heading);
    }
}
