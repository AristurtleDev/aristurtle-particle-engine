// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.Tests.Profiles;

public sealed class RingProfileTests
{
    [Fact]
    public void GetOffsetAndHeading_ReturnsOffsetEqualToRadius()
    {
        RingProfile profile = new RingProfile();
        profile.Radius = 10.0f;
        float[] values = new float[4];

        unsafe
        {
            fixed (float* offset = &values[0])
            {
                fixed (float* heading = &values[2])
                {
                    profile.GetOffsetAndHeading((Vector2*)offset, (Vector2*)heading);

                    float length = MathF.Sqrt((offset[0] * offset[0]) + (offset[1] * offset[1]));
                    Assert.Equal(10.0f, length, 0.000001f);
                }
            }
        }
    }

    [Fact]
    public void GetOffsetAndHeading_WhenRadiateIsOut_HeadingIsEqualToNormalizedOffset()
    {
        RingProfile profile = new RingProfile();
        profile.Radius = 10.0f;
        profile.Radiate = CircleRadiation.Out;
        float[] values = new float[4];

        unsafe
        {
            fixed (float* offset = &values[0])
            {
                fixed (float* heading = &values[2])
                {
                    profile.GetOffsetAndHeading((Vector2*)offset, (Vector2*)heading);

                    Assert.Equal(offset[0] / 10.0f, heading[0], 0.000001f);
                    Assert.Equal(offset[1] / 10.0f, heading[1], 0.000001f);
                }
            }
        }
    }
}
