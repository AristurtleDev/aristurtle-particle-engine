// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;
using Aristurtle.ParticleEngine.Profiles;
using Microsoft.Xna.Framework;

namespace Aristurtle.ParticleEngine.MonoGame.Sample.ParticleEffects;

public sealed class SmokeParticleEffect : ParticleEffect
{
    public SmokeParticleEffect() : base(nameof(SmokeParticleEffect))
    {
        Emitters = new List<ParticleEmitter>()
        {
            new ParticleEmitter(2000)
            {
                LifeSpan = 3.0f,
                Profile = Profile.Point(),
                AutoTrigger = false,
                Parameters = new ParticleReleaseParameters()
                {
                    Color = new ParticleColorParameter(new Vector3(0.0f, 0.0f, 0.6f).ToNumerics()),
                    Opacity = new ParticleFloatParameter(1.0f),
                    Quantity = new ParticleInt32Parameter(5),
                    Speed = new ParticleFloatParameter(0.0f, 100.0f),
                    Scale = new ParticleFloatParameter(32.0f),
                    Rotation = new ParticleFloatParameter(-MathF.PI, MathF.PI),
                    Mass = new ParticleFloatParameter(8.0f, 12.0f)
                },
                ReclaimFrequency = 5.0f,
                TextureKey = "Cloud001",
                Modifiers = new List<Modifier>()
                {
                    new DragModifier()
                    {
                        Frequency = 10.0f,
                        DragCoefficient = 0.47f,
                        Density = 0.125f
                    },
                    new RotationModifier()
                    {
                        Frequency = 15.0f,
                        RotationRate = 1.0f
                    },
                    new AgeModifier()
                    {
                        Frequency = 60.0f,
                        Interpolators = new List<Interpolator>()
                        {
                            new ScaleInterpolator()
                            {
                                StartValue = 0.5f,
                                EndValue = 1.0f
                            },
                            new OpacityInterpolator()
                            {
                                StartValue = 0.3f,
                                EndValue = 0.0f
                            }
                        }
                    }
                }
            }
        };
    }
}
