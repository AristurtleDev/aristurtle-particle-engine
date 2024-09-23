// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class CircleRadiationJsonConverter : JsonConverter<CircleRadiation>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(CircleRadiation);
    }

    public override CircleRadiation Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("JSON string expected");
        }

        string radiation = reader.GetString();

        return radiation switch
        {
            nameof(CircleRadiation.None) => CircleRadiation.None,
            nameof(CircleRadiation.In) => CircleRadiation.In,
            nameof(CircleRadiation.Out) => CircleRadiation.Out,
            _ => throw new JsonException($"Unknown {nameof(CircleRadiation)}: {radiation}")
        };
    }

    public override void Write(Utf8JsonWriter writer, CircleRadiation value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        switch (value)
        {
            case CircleRadiation.None:
                writer.WriteStringValue(nameof(CircleRadiation.None));
                break;

            case CircleRadiation.In:
                writer.WriteStringValue(nameof(CircleRadiation.In));
                break;

            case CircleRadiation.Out:
                writer.WriteStringValue(nameof(CircleRadiation.Out));
                break;

            default:
                throw new JsonException($"Unknown {nameof(CircleRadiation)}: {value}");
        }
    }
}
