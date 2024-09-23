// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Serialization.Json;

public static class ParticleEffectJsonSerializerOptionsProvider
{
    public static readonly JsonSerializerOptions Default;

    static ParticleEffectJsonSerializerOptionsProvider()
    {
        Default = new JsonSerializerOptions();
        Default.WriteIndented = true;
        Default.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        Default.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        Default.Converters.Add(new CircleRadiationJsonConverter());
        Default.Converters.Add(new InterpolatorJsonConverter());
        Default.Converters.Add(new ModifierExecutionStrategyConverter());
        Default.Converters.Add(new ModifierJsonConverter());
        Default.Converters.Add(new ParticleColorParameterConverter());
        Default.Converters.Add(new ParticleEffectJsonConverter());
        Default.Converters.Add(new ParticleEmitterJsonConverter());
        Default.Converters.Add(new ParticleFloatParameterConverter());
        Default.Converters.Add(new ParticleInt32ParameterConverter());
        Default.Converters.Add(new ParticleReleaseParametersConverter());
        Default.Converters.Add(new ParticleRenderingOrderConverter());
        Default.Converters.Add(new ParticleValueKindConverter());
        Default.Converters.Add(new ParticleVector2ParameterConverter());
        Default.Converters.Add(new ProfileJsonConverter());
        Default.Converters.Add(new RectangleJsonConverter());
        Default.Converters.Add(new Vector2JsonConverter());
        Default.Converters.Add(new Vector3JsonConverter());
    }
}
