// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using Aristurtle.ParticleEngine;
using Aristurtle.ParticleEngine.Data;
using Aristurtle.ParticleEngine.Modifiers.Containers;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Profiles;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Aristurtle.ParticleEngine.Serialization.Json;

ParticleEffect effect = new ParticleEffect("Ring");

ParticleEmitter emitter = new ParticleEmitter(2000);
emitter.AutoTrigger = false;
emitter.LifeSpan = 3.0f;
emitter.Profile = Profile.Spray(-Vector2.UnitY, 0.5f);
emitter.ReclaimFrequency = 0.5f;
emitter.TextureKey = "Ring001";

emitter.Parameters.Opacity = new ParticleFloatParameter(1.0f);
emitter.Parameters.Quantity = new ParticleInt32Parameter(1);
emitter.Parameters.Speed = new ParticleFloatParameter(300.0f, 700.0f);
emitter.Parameters.Scale = new ParticleFloatParameter(0.5f);
emitter.Parameters.Mass = new ParticleFloatParameter(4.0f, 12.0f);
emitter.Parameters.Color = new ParticleColorParameter(new Vector3(210.0f, 0.5f, 0.6f), new Vector3(230.0f, 0.7f, 0.8f));

LinearGravityModifier gravityModifier = new LinearGravityModifier();
gravityModifier.Direction = -Vector2.UnitX;
gravityModifier.Strength = 100.0f;
gravityModifier.Frequency = 20.0f;

OpacityFastFadeModifier fadeModifier = new OpacityFastFadeModifier();
fadeModifier.Frequency = 10.0f;

RectangleContainerModifier containerModifier = new RectangleContainerModifier();
containerModifier.Frequency = 15.0f;
containerModifier.Width = 1280;
containerModifier.Height = 720;
containerModifier.RestitutionCoefficient = 0.75f;

emitter.Modifiers.Add(gravityModifier);
emitter.Modifiers.Add(fadeModifier);
emitter.Modifiers.Add(containerModifier);

effect.Emitters.Add(emitter);


JsonSerializerOptions options = ParticleEffectJsonSerializerOptionsProvider.Default;
string json = JsonSerializer.Serialize<ParticleEffect>(effect, options);
File.WriteAllText("ring.json", json);


