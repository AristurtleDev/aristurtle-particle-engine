// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;

namespace Aristurtle.ParticleEngine.Profiles;

public sealed class CircleProfile : Profile
{
    public float Radius;
    public CircleRadiation Radiate;

    public override unsafe void GetOffsetAndHeading(Vector2* offset, Vector2* heading)
    {
        float distance = FastRandom.NextSingle(0f, Radius);

        FastRandom.NextUnitVector(heading);

        switch (Radiate)
        {
            case CircleRadiation.In:
                offset->X = -heading->X * distance;
                offset->Y = -heading->Y * distance;
                break;

            case CircleRadiation.Out:
                offset->X = heading->X * distance;
                offset->Y = heading->Y * distance;
                break;

            case CircleRadiation.None:
                offset->X = heading->X * distance;
                offset->Y = heading->Y * distance;
                FastRandom.NextUnitVector(heading);
                break;

            default:
                throw new ArgumentOutOfRangeException($"{Radiate} is not supported");
        }
    }
}
