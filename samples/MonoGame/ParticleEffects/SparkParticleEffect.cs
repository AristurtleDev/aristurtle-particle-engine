// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Modifiers.Containers;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.MonoGame.Sample.ParticleEffects;

public sealed class SparkParticleEffect : ParticleEffect
{
    public SparkParticleEffect() : base(nameof(SparkParticleEffect))
    {
        Emitters = new List<ParticleEmitter>()
        {
            new ParticleEmitter(2000)
            {
                AutoTrigger = false,
                LifeSpan = 2.0f,
                Profile = Profile.Point(),
                Parameters = new ParticleReleaseParameters()
                {
                    Opacity = new ParticleFloatParameter(1.0f),
                    Quantity = new ParticleInt32Parameter(10),
                    Speed = new ParticleFloatParameter(0.0f, 100.0f),
                    Scale = new ParticleFloatParameter(1.0f),
                    Mass = new ParticleFloatParameter(8.0f, 12.0f),
                    Color = new ParticleColorParameter(new Vector3(50.0f, 0.8f, 0.5f))
                },
                ReclaimFrequency = 5.0f,
                TextureName = "Particle",
                Modifiers = new List<Modifier>()
                {
                    new LinearGravityModifier()
                    {
                        Direction = Vector2.UnitY,
                        Strength = 30.0f,
                        Frequency = 15.0f
                    },
                    new RectangleContainerModifier()
                    {
                        Frequency = 15.0f,
                        Width = 1280,
                        Height = 720,
                        RestitutionCoefficient = 0.75f,
                    },
                    new OpacityFastFadeModifier()
                    {
                        Frequency = 10.0f
                    }
                }
            }
        };
    }
}
