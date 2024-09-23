// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal class ParticleFloatParameterConverter : JsonConverter<ParticleFloatParameter>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleFloatParameter);
    }

    public override ParticleFloatParameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON object expected");
        }

        ParticleValueKind kind = ParticleValueKind.Constant;
        float constant = default;
        float randomMin = default;
        float randomMax = default;

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
                case nameof(ParticleColorParameter.Kind):
                    kind = JsonSerializer.Deserialize<ParticleValueKind>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.Constant):
                    constant = JsonSerializer.Deserialize<float>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.RandomMin):
                    randomMin = JsonSerializer.Deserialize<float>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.RandomMax):
                    randomMax = JsonSerializer.Deserialize<float>(ref reader, options);
                    break;

                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }

        return kind switch
        {
            ParticleValueKind.Constant => new ParticleFloatParameter(constant),
            ParticleValueKind.Random => new ParticleFloatParameter(randomMin, randomMax),
            _ => throw new JsonException($"Unexpected ParticleValueKind: {kind}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ParticleFloatParameter value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ParticleFloatParameter.Kind));
        JsonSerializer.Serialize(writer, value.Kind, options);

        if (value.Kind == ParticleValueKind.Constant)
        {
            writer.WritePropertyName(nameof(ParticleFloatParameter.Constant));
            JsonSerializer.Serialize(writer, value.Constant, options);
        }
        else if (value.Kind == ParticleValueKind.Random)
        {
            writer.WritePropertyName(nameof(ParticleFloatParameter.RandomMin));
            JsonSerializer.Serialize(writer, value.RandomMin, options);

            writer.WritePropertyName(nameof(ParticleFloatParameter.RandomMax));
            JsonSerializer.Serialize(writer, value.RandomMax, options);
        }

        writer.WriteEndObject();
    }
}
