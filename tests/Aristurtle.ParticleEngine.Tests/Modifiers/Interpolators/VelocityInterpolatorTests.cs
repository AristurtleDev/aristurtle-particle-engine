// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;

namespace Aristurtle.ParticleEngine.Tests.Modifiers.Interpolators;

public sealed class VelocityInterpolatorTests
{
    [Fact]
    public void Update_WhenAmountIsZero_SetsStartValue()
    {
        Particle particle = new Particle();

        VelocityInterpolator interpolator = new VelocityInterpolator();
        interpolator.StartValue = new Vector2(1.0f, 0.0f);
        interpolator.EndValue = new Vector2(0.0f, 1.0f);

        unsafe
        {
            interpolator.Update(0.0f, &particle);

            Assert.Equal(1.0f, particle.Velocity[0], 0.000001f);
            Assert.Equal(0.0f, particle.Velocity[1], 0.000001f);
        }
    }

    [Fact]
    public void Update_WhenAmountIsOne_SetsEndValue()
    {
        Particle particle = new Particle();

        VelocityInterpolator interpolator = new VelocityInterpolator();
        interpolator.StartValue = new Vector2(1.0f, 0.0f);
        interpolator.EndValue = new Vector2(0.0f, 1.0f);

        unsafe
        {
            interpolator.Update(1.0f, &particle);

            Assert.Equal(0.0f, particle.Velocity[0], 0.000001f);
            Assert.Equal(1.0f, particle.Velocity[1], 0.000001f);
        }
    }

    [Fact]
    public void Update_WhenAmountIsHalf_SetHalfValue()
    {
        Particle particle = new Particle();

        VelocityInterpolator interpolator = new VelocityInterpolator();
        interpolator.StartValue = new Vector2(1.0f, 0.0f);
        interpolator.EndValue = new Vector2(0.0f, 1.0f);

        unsafe
        {
            interpolator.Update(0.5f, &particle);

            Assert.Equal(0.5f, particle.Velocity[0], 0.000001f);
            Assert.Equal(0.5f, particle.Velocity[1], 0.000001f);
        }
    }
}
