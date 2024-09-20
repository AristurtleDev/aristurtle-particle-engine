// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Modifiers.Containers;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.MonoGame.Sample.ParticleEffects;

public sealed class LoadTestParticleEffect : ParticleEffect
{
    public LoadTestParticleEffect() : base(nameof(LoadTestParticleEffect))
    {
        Emitters = new List<ParticleEmitter>()
        {
            new ParticleEmitter(1000000)
            {
                AutoTrigger = false,
                LifeSpan = 10,
                Profile = Profile.Line(Vector2.UnitX, 10.0f),
                Parameters = new ParticleReleaseParameters()
                {
                    Quantity = new ParticleInt32Parameter(10000),
                    Speed = new ParticleFloatParameter(0.0f, 200.0f),
                    Scale = new ParticleFloatParameter(1.0f),
                    Mass = new ParticleFloatParameter(4.0f, 12.0f),
                    Opacity = new ParticleFloatParameter(0.4f),
                },
                ReclaimFrequency = 5.0f,
                TextureName = "Pixel",
                ModifierExecutionStrategy = ModifierExecutionStrategy.Parallel,
                Modifiers = new List<Modifier>()
                {
                    new LinearGravityModifier()
                    {
                        Direction = Vector2.UnitY,
                        Strength = 30.0f,
                        Frequency = 15.0f
                    },
                    new OpacityFastFadeModifier()
                    {
                        Frequency = 10.0f
                    },
                    new RectangleContainerModifier()
                    {
                        Frequency = 15.0f,
                        Width = 1280,
                        Height = 720,
                        RestitutionCoefficient = 0.75f,
                    },
                    new DragModifier()
                    {
                        Frequency = 10.0f,
                        DragCoefficient = 0.47f,
                        Density = 0.125f
                    },
                    new AgeModifier()
                    {
                        Frequency = 10.0f,
                        Interpolators = new List<Interpolator>()
                        {
                            new HueInterpolator()
                            {
                                StartValue = 0.0f,
                                EndValue = 150.0f
                            }
                        }
                    },
                }
            }
        };
    }
}
