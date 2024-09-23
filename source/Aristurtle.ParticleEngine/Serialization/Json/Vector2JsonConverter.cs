// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class Vector2JsonConverter : JsonConverter<Vector2>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Vector2);
    }

    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("JSON string expected");
        }

        string value = reader.GetString();

        if(string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Unexpected empty or null string");
        }

        string separator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        string[] xy = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        if (xy.Length != 2)
        {
            throw new JsonException($"Invalid format, expected 'x{separator}y', got '{value}'");
        }

        if (!float.TryParse(xy[0], CultureInfo.InvariantCulture, out float x))
        {
            throw new JsonException($"Invalid format, expected float, got '{xy[0]}'");
        }

        if (!float.TryParse(xy[1], CultureInfo.InvariantCulture, out float y))
        {
            throw new JsonException($"Invalid format, expected float, got '{xy[1]}'");
        }

        return new Vector2(x, y);
    }

    public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        string format = "{1}{0}{2}";
        string separator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        string output = string.Format(CultureInfo.InvariantCulture, format, separator, value.X, value.Y);
        writer.WriteStringValue(output);
    }
}
