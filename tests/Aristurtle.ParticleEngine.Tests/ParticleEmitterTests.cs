// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.Tests;

public sealed class ParticleEmitterTests
{
    [Fact]
    public void WhenThereAreParticlesToExpire_DecreaseActiveParticleCount()
    {
        ParticleEmitter emitter = new ParticleEmitter(100);
        emitter.Profile = Profile.Point();
        emitter.LifeSpan = 1.0f;
        emitter.Parameters.Quantity = new ParticleInt32Parameter(1);
        emitter.AutoTrigger = false;

        emitter.Trigger(Vector2.Zero);
        Assert.Equal(1, emitter.ActiveParticles);

        emitter.Update(2.0f);

        Assert.Equal(0, emitter.ActiveParticles);
    }

    [Fact]
    public void WhenThereAreParticlesToExpire_DoesNotPassExpiredParticlesToModifiers()
    {
        ParticleEmitter emitter = new ParticleEmitter(100);
        emitter.LifeSpan = 1.0f;
        emitter.Profile = Profile.Point();
        emitter.Parameters.Quantity = new ParticleInt32Parameter(1);
        emitter.AutoTrigger = false;
        emitter.Modifiers.Add(new AssertionModifier(particle => particle.Age <= 1.0f));

        emitter.Trigger(Vector2.Zero);
        emitter.Update(0.5f);
        emitter.Trigger(Vector2.Zero);
        emitter.Update(0.5f);
        emitter.Trigger(Vector2.Zero);
        emitter.Update(0.5f);
    }

    [Fact]
    public void WhenThereAreNoActiveParticles_GracefullyDoesNothing()
    {
        ParticleEmitter emitter = new ParticleEmitter(100);
        emitter.LifeSpan = 1.0f;
        emitter.Profile = Profile.Point();
        emitter.AutoTrigger = false;

        emitter.Update(0.5f);

        Assert.Equal(0, emitter.ActiveParticles);
    }

    [Fact]
    public void WhenEnoughHeadroom_IncreasesActiveParticlesCountByReleaseQuantity()
    {
        ParticleEmitter emitter = new ParticleEmitter(100);
        emitter.LifeSpan = 1.0f;
        emitter.Profile = Profile.Point();
        emitter.AutoTrigger = false;
        emitter.Parameters.Quantity = new ParticleInt32Parameter(10);

        Assert.Equal(0, emitter.ActiveParticles);
        emitter.Trigger(Vector2.Zero);
        Assert.Equal(10, emitter.ActiveParticles);
    }

    [Fact]
    public void WhenNotEnoughHeadroom_IncreasesActiveParticlesCountByRemainingParticles()
    {
        ParticleEmitter emitter = new ParticleEmitter(15);
        emitter.LifeSpan = 1.0f;
        emitter.Profile = Profile.Point();
        emitter.AutoTrigger = false;
        emitter.Parameters.Quantity = new ParticleInt32Parameter(10);

        emitter.Trigger(Vector2.Zero);
        Assert.Equal(10, emitter.ActiveParticles);

        emitter.Trigger(Vector2.Zero);
        Assert.Equal(15, emitter.ActiveParticles);
    }

    [Fact]
    public void WhenNoRemainingParticles_DoesNotIncreaseActiveParticlesCount()
    {
        ParticleEmitter emitter = new ParticleEmitter(10);
        emitter.LifeSpan = 1.0f;
        emitter.Profile = Profile.Point();
        emitter.AutoTrigger = false;
        emitter.Parameters.Quantity = new ParticleInt32Parameter(10);

        emitter.Trigger(Vector2.Zero);
        Assert.Equal(10, emitter.ActiveParticles);

        emitter.Trigger(Vector2.Zero);
        Assert.Equal(10, emitter.ActiveParticles);
    }

    [Fact]
    public void WhenDisposingMultipleTimes_DoesNotThrowException()
    {
        ParticleEmitter emitter = new ParticleEmitter(10);

        Assert.Null(Record.Exception(() => emitter.Dispose()));
        Assert.Null(Record.Exception(() => emitter.Dispose()));
    }

}
