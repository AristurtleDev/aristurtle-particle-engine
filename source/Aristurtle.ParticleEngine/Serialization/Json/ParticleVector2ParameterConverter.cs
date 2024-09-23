// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal class ParticleVector2ParameterConverter : JsonConverter<ParticleVector2Parameter>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleVector2Parameter);
    }

    public override ParticleVector2Parameter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON object expected");
        }

        ParticleValueKind kind = ParticleValueKind.Constant;
        Vector2 constant = default;
        Vector2 randomMin = default;
        Vector2 randomMax = default;

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
                    constant = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.RandomMin):
                    randomMin = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                    break;

                case nameof(ParticleColorParameter.RandomMax):
                    randomMax = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                    break;

                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }

        return kind switch
        {
            ParticleValueKind.Constant => new ParticleVector2Parameter(constant),
            ParticleValueKind.Random => new ParticleVector2Parameter(randomMin, randomMax),
            _ => throw new JsonException($"Unexpected {nameof(ParticleValueKind)}: {kind}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ParticleVector2Parameter value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ParticleVector2Parameter.Kind));
        JsonSerializer.Serialize(writer, value.Kind, options);

        switch (value.Kind)
        {
            case ParticleValueKind.Constant:
                writer.WritePropertyName(nameof(ParticleVector2Parameter.Constant));
                JsonSerializer.Serialize(writer, value.Constant, options);
                break;

            case ParticleValueKind.Random:
                writer.WritePropertyName(nameof(ParticleVector2Parameter.RandomMin));
                JsonSerializer.Serialize(writer, value.RandomMin, options);
                writer.WritePropertyName(nameof(ParticleVector2Parameter.RandomMax));
                JsonSerializer.Serialize(writer, value.RandomMax, options);
                break;

            default:
                throw new JsonException($"Unexpected {nameof(ParticleValueKind)}: {value.Kind}");
        }

        writer.WriteEndObject();
    }
}
