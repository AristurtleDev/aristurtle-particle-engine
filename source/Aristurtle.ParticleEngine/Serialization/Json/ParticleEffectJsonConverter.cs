// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ParticleEffectJsonConverter : JsonConverter<ParticleEffect>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleEffect);
    }

    public override ParticleEffect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON object expected");
        }

        ParticleEffect effect = new ParticleEffect(string.Empty);

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
                case nameof(ParticleEffect.Name):
                    effect.Name = reader.GetString();
                    break;

                case nameof(ParticleEffect.Position):
                    effect.Position = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                    break;

                case nameof(ParticleEffect.Rotation):
                    effect.Rotation = reader.GetSingle();
                    break;

                case nameof(ParticleEffect.Scale):
                    effect.Scale= JsonSerializer.Deserialize<Vector2>(ref reader, options);
                    break;

                case nameof(ParticleEffect.Emitters):
                    effect.Emitters = JsonSerializer.Deserialize<List<ParticleEmitter>>(ref reader, options);
                    break;

                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }

        return effect;
    }

    public override void Write(Utf8JsonWriter writer, ParticleEffect value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        writer.WriteString(nameof(ParticleEffect.Name), value.Name);

        writer.WritePropertyName(nameof(ParticleEffect.Position));
        JsonSerializer.Serialize(writer, value.Position, options);

        writer.WriteNumber(nameof(ParticleEffect.Rotation), value.Rotation);

        writer.WritePropertyName(nameof(ParticleEffect.Scale));
        JsonSerializer.Serialize(writer, value.Scale, options);

        writer.WritePropertyName(nameof(ParticleEffect.Emitters));
        JsonSerializer.Serialize(writer, value.Emitters, options);

        writer.WriteEndObject();
    }
}
