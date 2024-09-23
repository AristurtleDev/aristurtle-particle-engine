// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Drawing;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ParticleEmitterJsonConverter : JsonConverter<ParticleEmitter>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleEmitter);
    }

    public override ParticleEmitter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("JSON object expected");
        }

        ParticleEmitter emitter = new ParticleEmitter();

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
                case nameof(ParticleEmitter.Name):
                    emitter.Name = reader.GetString();
                    break;

                case nameof(ParticleEmitter.Capacity):
                    int capacity = reader.GetInt32();
                    emitter.ChangeCapacity(capacity);
                    break;

                case nameof(ParticleEmitter.LifeSpan):
                    emitter.LifeSpan = reader.GetSingle();
                    break;

                case nameof(ParticleEmitter.Offset):
                    emitter.Offset = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                    break;

                case nameof(ParticleEmitter.LayerDepth):
                    emitter.LayerDepth = reader.GetSingle();
                    break;

                case nameof(ParticleEmitter.AutoTrigger):
                    emitter.AutoTrigger = reader.GetBoolean();
                    break;

                case nameof(ParticleEmitter.AutoTriggerFrequency):
                    emitter.AutoTriggerFrequency = reader.GetSingle();
                    break;

                case nameof(ParticleEmitter.ReclaimFrequency):
                    emitter.ReclaimFrequency = reader.GetSingle();
                    break;

                case nameof(ParticleEmitter.Parameters):
                    emitter.Parameters = JsonSerializer.Deserialize<ParticleReleaseParameters>(ref reader, options);
                    break;

                case nameof(ParticleEmitter.ModifierExecutionStrategy):
                    emitter.ModifierExecutionStrategy = JsonSerializer.Deserialize<ModifierExecutionStrategy>(ref reader, options);
                    break;

                case nameof(ParticleEmitter.Modifiers):
                    emitter.Modifiers = JsonSerializer.Deserialize<List<Modifier>>(ref reader, options);
                    break;

                case nameof(ParticleEmitter.Profile):
                    emitter.Profile = JsonSerializer.Deserialize<Profile>(ref reader, options);
                    break;

                case nameof(ParticleEmitter.TextureKey):
                    emitter.TextureKey = reader.GetString();
                    break;

                case nameof(ParticleEmitter.SourceRectangle):
                    emitter.SourceRectangle = JsonSerializer.Deserialize<Rectangle>(ref reader, options);
                    break;

                case nameof(ParticleEmitter.RenderingOrder):
                    emitter.RenderingOrder = JsonSerializer.Deserialize<ParticleRenderingOrder>(ref reader, options);
                    break;

                default:
                    throw new JsonException($"Unexpected property: {propertyName}");
            }
        }

        return emitter;
    }

    public override void Write(Utf8JsonWriter writer, ParticleEmitter value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        writer.WriteString(nameof(ParticleEmitter.Name), value.Name);
        writer.WriteNumber(nameof(ParticleEmitter.Capacity), value.Capacity);
        writer.WriteNumber(nameof(ParticleEmitter.LifeSpan), value.LifeSpan);

        writer.WritePropertyName(nameof(ParticleEmitter.Offset));
        JsonSerializer.Serialize(writer, value.Offset, options);

        writer.WriteNumber(nameof(ParticleEmitter.LayerDepth), value.LayerDepth);
        writer.WriteBoolean(nameof(ParticleEmitter.AutoTrigger), value.AutoTrigger);
        writer.WriteNumber(nameof(ParticleEmitter.AutoTriggerFrequency), value.AutoTriggerFrequency);
        writer.WriteNumber(nameof(ParticleEmitter.ReclaimFrequency), value.ReclaimFrequency);

        writer.WritePropertyName(nameof(ParticleEmitter.Parameters));
        JsonSerializer.Serialize(writer, value.Parameters, options);

        writer.WritePropertyName(nameof(ParticleEmitter.ModifierExecutionStrategy));
        JsonSerializer.Serialize(writer, value.ModifierExecutionStrategy, options);

        writer.WritePropertyName(nameof(ParticleEmitter.Modifiers));
        JsonSerializer.Serialize(writer, value.Modifiers, options);

        writer.WritePropertyName(nameof(ParticleEmitter.Profile));
        JsonSerializer.Serialize(writer, value.Profile, options);

        writer.WriteString(nameof(ParticleEmitter.TextureKey), value.TextureKey);

        writer.WritePropertyName(nameof(ParticleEmitter.SourceRectangle));
        JsonSerializer.Serialize(writer, value.SourceRectangle, options);

        writer.WritePropertyName(nameof(ParticleEmitter.RenderingOrder));
        JsonSerializer.Serialize(writer, value.RenderingOrder, options);

        writer.WriteEndObject();
    }
}
