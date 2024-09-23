// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ParticleInt32ParameterConverter : JsonConverter<ParticleInt32Parameter>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleInt32Parameter);
    }

    public override ParticleInt32Parameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON object expected");
        }

        ParticleValueKind kind = ParticleValueKind.Constant;
        int constant = default;
        int randomMin = default;
        int randomMax = default;

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
                    constant = JsonSerializer.Deserialize<int>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.RandomMin):
                    randomMin = JsonSerializer.Deserialize<int>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.RandomMax):
                    randomMax = JsonSerializer.Deserialize<int>(ref reader, options);
                    break;

                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }

        return kind switch
        {
            ParticleValueKind.Constant => new ParticleInt32Parameter(constant),
            ParticleValueKind.Random => new ParticleInt32Parameter(randomMin, randomMax),
            _ => throw new JsonException($"Unexpected ParticleValueKind: {kind}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ParticleInt32Parameter value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ParticleInt32Parameter.Kind));
        JsonSerializer.Serialize(writer, value.Kind, options);

        if (value.Kind == ParticleValueKind.Constant)
        {
            writer.WritePropertyName(nameof(ParticleInt32Parameter.Constant));
            JsonSerializer.Serialize(writer, value.Constant, options);
        }
        else if (value.Kind == ParticleValueKind.Random)
        {
            writer.WritePropertyName(nameof(ParticleInt32Parameter.RandomMin));
            JsonSerializer.Serialize(writer, value.RandomMin, options);

            writer.WritePropertyName(nameof(ParticleInt32Parameter.RandomMax));
            JsonSerializer.Serialize(writer, value.RandomMax, options);
        }

        writer.WriteEndObject();
    }
}
