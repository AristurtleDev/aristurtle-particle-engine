// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Tests;

public sealed class ParticleBufferTests
{
    [Fact]
    public void AvailableProperty_WhenNoParticlesReleased_ReturnsBufferSize()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);
        Assert.Equal(100, buffer.Available);
    }

    [Fact]
    public void AvailableProperty_WhenSomeParticlesReleased_ReturnsAvailableCount()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            buffer.Release(10, out _);
        }

        Assert.Equal(90, buffer.Available);
    }

    [Fact]
    public void AvailableProperty_WhenAllParticlesReleased_ReturnsZero()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            buffer.Release(100, out _);
        }

        Assert.Equal(0, buffer.Available);
    }

    [Fact]
    public void CountProperty_WhenNoParticlesReleased_ReturnsZero()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);
        Assert.Equal(0, buffer.Count);
    }

    [Fact]
    public void CountProperty_WhenSomeParticlesReleased_ReturnsCount()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            buffer.Release(10, out _);
        }

        Assert.Equal(10, buffer.Count);
    }

    [Fact]
    public void CountProperty_WhenAllParticlesReleased_ReturnsZero()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            buffer.Release(100, out _);
        }

        Assert.Equal(100, buffer.Count);
    }

    [Fact]
    public void ReleaseMethod_WhenPassedReasonableQuantity_ReturnsNumberReleased()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            int count = buffer.Release(50, out _);
            Assert.Equal(50, count);
        }
    }

    [Fact]
    public void ReleaseMethod_WhenPassedImpossibleQuantity_ReturnsNumberActuallyReleased()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            int count = buffer.Release(200, out _);
            Assert.Equal(100, count);
        }
    }

    [Fact]
    public void ReclaimMethod_WhenPassedReasonableNumber_ReclaimsParticles()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);

        unsafe
        {
            buffer.Release(100, out _);
        }

        Assert.Equal(100, buffer.Count);

        buffer.Reclaim(50);

        Assert.Equal(50, buffer.Count);
    }

    [Fact]
    public void CopyToMethod_WhenBufferIsSequential_CopiesParticlesInOrder()
    {
        unsafe
        {
            ParticleBuffer buffer = new ParticleBuffer(10);
            int count = buffer.Release(5, out Particle* particle);

            do
            {
                particle->Age = 1f;
                particle++;
            }
            while (count-- > 0);

            Particle[] destination = new Particle[10];

            fixed (Particle* particles = destination)
            {
                buffer.CopyTo((IntPtr)particles);
            }

            Assert.Equal(1.0f, destination[0].Age, 0.000001f);
            Assert.Equal(1.0f, destination[1].Age, 0.000001f);
            Assert.Equal(1.0f, destination[2].Age, 0.000001f);
            Assert.Equal(1.0f, destination[3].Age, 0.000001f);
            Assert.Equal(1.0f, destination[4].Age, 0.000001f);
        }
    }

    [Fact]
    public void CopyToReverseMethod_WhenBufferIsSequential_CopiesParticlesInReverseOrder()
    {
        unsafe
        {
            ParticleBuffer buffer = new ParticleBuffer(10);
            int count = buffer.Release(5, out Particle* particle);

            do
            {
                particle->Age = 1f;
                particle++;
            }
            while (count-- > 0);

            var destination = new Particle[10];

            fixed (Particle* particles = destination)
            {
                buffer.CopyToReverse((IntPtr)particles);
            }

            Assert.Equal(1.0f, destination[0].Age, 0.000001f);
            Assert.Equal(1.0f, destination[1].Age, 0.000001f);
            Assert.Equal(1.0f, destination[2].Age, 0.000001f);
            Assert.Equal(1.0f, destination[3].Age, 0.000001f);
            Assert.Equal(1.0f, destination[4].Age, 0.000001f);
        }
    }

    [Fact]
    public void DisposeMethod_WhenDisposingMultipleTimes_DoesNotThrowException()
    {
        ParticleBuffer buffer = new ParticleBuffer(100);
        Assert.Null(Record.Exception(() => buffer.Dispose()));
        Assert.Null(Record.Exception(() => buffer.Dispose()));
    }
}
