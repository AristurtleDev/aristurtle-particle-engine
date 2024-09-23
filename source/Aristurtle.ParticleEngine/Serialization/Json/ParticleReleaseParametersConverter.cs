// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ParticleReleaseParametersConverter : JsonConverter<ParticleReleaseParameters>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleReleaseParameters);
    }

    public override ParticleReleaseParameters Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON object expected");
        }

        ParticleReleaseParameters parameters = new ParticleReleaseParameters();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Property name expected");
            }

            string propertyName = reader.GetString();
            reader.Read();

            switch (propertyName)
            {
                case nameof(ParticleReleaseParameters.Color):
                    parameters.Color = JsonSerializer.Deserialize<ParticleColorParameter>(ref reader, options);
                    break;

                case nameof(ParticleReleaseParameters.Mass):
                    parameters.Mass = JsonSerializer.Deserialize<ParticleFloatParameter>(ref reader, options);
                    break;

                case nameof(ParticleReleaseParameters.Opacity):
                    parameters.Opacity = JsonSerializer.Deserialize<ParticleFloatParameter>(ref reader, options);
                    break;

                case nameof(ParticleReleaseParameters.Quantity):
                    parameters.Quantity = JsonSerializer.Deserialize<ParticleInt32Parameter>(ref reader, options);
                    break;

                case nameof(ParticleReleaseParameters.Rotation):
                    parameters.Rotation = JsonSerializer.Deserialize<ParticleFloatParameter>(ref reader, options);
                    break;

                case nameof(ParticleReleaseParameters.Scale):
                    parameters.Scale = JsonSerializer.Deserialize<ParticleFloatParameter>(ref reader, options);
                    break;

                case nameof(ParticleReleaseParameters.Speed):
                    parameters.Speed = JsonSerializer.Deserialize<ParticleFloatParameter>(ref reader, options);
                    break;

                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }

        return parameters;
    }

    public override void Write(Utf8JsonWriter writer, ParticleReleaseParameters value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Color));
        JsonSerializer.Serialize(writer, value.Color, options);

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Mass));
        JsonSerializer.Serialize(writer, value.Mass, options);

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Opacity));
        JsonSerializer.Serialize(writer, value.Opacity, options);

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Quantity));
        JsonSerializer.Serialize(writer, value.Quantity, options);

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Rotation));
        JsonSerializer.Serialize(writer, value.Rotation, options);

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Scale));
        JsonSerializer.Serialize(writer, value.Scale, options);

        writer.WritePropertyName(nameof(ParticleReleaseParameters.Speed));
        JsonSerializer.Serialize(writer, value.Speed, options);

        writer.WriteEndObject();
    }
}
