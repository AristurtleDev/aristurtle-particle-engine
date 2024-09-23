// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ParticleRenderingOrderConverter : JsonConverter<ParticleRenderingOrder>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleRenderingOrder);
    }

    public override ParticleRenderingOrder Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("JSON string expected");
        }

        string renderingOrder = reader.GetString();

        return renderingOrder switch
        {
            nameof(ParticleRenderingOrder.FrontToBack) => ParticleRenderingOrder.FrontToBack,
            nameof(ParticleRenderingOrder.BackToFront) => ParticleRenderingOrder.BackToFront,
            _ => throw new JsonException($"Unknown {nameof(ParticleRenderingOrder)}: {renderingOrder}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ParticleRenderingOrder value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        switch (value)
        {
            case ParticleRenderingOrder.FrontToBack:
                writer.WriteStringValue(nameof(ParticleRenderingOrder.FrontToBack));
                break;

            case ParticleRenderingOrder.BackToFront:
                writer.WriteStringValue(nameof(ParticleRenderingOrder.BackToFront));
                break;

            default:
                throw new JsonException($"Unknown {nameof(ParticleRenderingOrder)}: {value}");
        }
    }
}
