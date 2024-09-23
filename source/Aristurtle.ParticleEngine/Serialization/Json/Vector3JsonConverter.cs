// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class Vector3JsonConverter : JsonConverter<Vector3>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Vector3);
    }

    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("JSON string expected");
        }

        string value = reader.GetString();

        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Unexpected empty or null string");
        }

        string separator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        string[] xyz = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        if (xyz.Length != 3)
        {
            throw new JsonException($"Invalid format, expected 'x{separator}y{separator}z', got '{value}'");
        }

        if (!float.TryParse(xyz[0], CultureInfo.InvariantCulture, out float x))
        {
            throw new JsonException($"Invalid format, expected float, got '{xyz[0]}'");
        }

        if (!float.TryParse(xyz[1], CultureInfo.InvariantCulture, out float y))
        {
            throw new JsonException($"Invalid format, expected float, got '{xyz[1]}'");
        }

        if (!float.TryParse(xyz[2], CultureInfo.InvariantCulture, out float z))
        {
            throw new JsonException($"Invalid format, expected float, got '{xyz[2]}'");
        }

        return new Vector3(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        string format = "{1}{0}{2}{0}{3}";
        string separator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        string output = string.Format(CultureInfo.InvariantCulture, format, separator, value.X, value.Y, value.Z);
        writer.WriteStringValue(output);
    }
}
