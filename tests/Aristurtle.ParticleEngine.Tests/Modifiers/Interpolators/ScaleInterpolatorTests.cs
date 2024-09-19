// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;

namespace Aristurtle.ParticleEngine.Tests.Modifiers.Interpolators;

public sealed class ScaleInterpolatorTests
{
    [Fact]
    public void Update_WhenAmountIsZero_SetsStartValue()
    {
        Particle particle = new Particle();

        ScaleInterpolator interpolator = new ScaleInterpolator();
        interpolator.StartValue = 1.0f;
        interpolator.EndValue = 0.0f;

        unsafe
        {
            interpolator.Update(0.0f, &particle);

            Assert.Equal(1.0f, particle.Scale, 0.000001f);
        }
    }

    [Fact]
    public void Update_WhenAmountIsOne_SetsEndValue()
    {
        Particle particle = new Particle();

        ScaleInterpolator interpolator = new ScaleInterpolator();
        interpolator.StartValue = 1.0f;
        interpolator.EndValue = 0.0f;

        unsafe
        {
            interpolator.Update(1.0f, &particle);

            Assert.Equal(0.0f, particle.Scale, 0.000001f);
        }
    }

    [Fact]
    public void Update_WhenAmountIsHalf_SetHalfValue()
    {
        Particle particle = new Particle();

        ScaleInterpolator interpolator = new ScaleInterpolator();
        interpolator.StartValue = 1.0f;
        interpolator.EndValue = 0.0f;

        unsafe
        {
            interpolator.Update(0.5f, &particle);

            Assert.Equal(0.5f, particle.Scale, 0.000001f);
        }
    }
}
