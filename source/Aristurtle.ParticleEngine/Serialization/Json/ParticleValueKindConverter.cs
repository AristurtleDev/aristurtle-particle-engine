// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Data;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ParticleValueKindConverter : JsonConverter<ParticleValueKind>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ParticleValueKind);
    }

    public override ParticleValueKind Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("JSON string value expected");
        }

        string kind = reader.GetString();

        return kind switch
        {
            nameof(ParticleValueKind.Constant) => ParticleValueKind.Constant,
            nameof(ParticleValueKind.Random) => ParticleValueKind.Random,
            _ => throw new JsonException($"Unexpected {nameof(ParticleValueKind)}: {kind}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ParticleValueKind value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        switch (value)
        {
            case ParticleValueKind.Constant:
                writer.WriteStringValue(nameof(ParticleValueKind.Constant));
                break;

            case ParticleValueKind.Random:
                writer.WriteStringValue(nameof(ParticleValueKind.Random));
                break;

            default:
                throw new JsonException($"Unexpected {nameof(ParticleValueKind)}: {value}");
        }
    }
}
