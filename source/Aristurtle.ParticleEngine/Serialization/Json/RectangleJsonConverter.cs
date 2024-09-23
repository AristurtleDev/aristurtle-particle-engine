// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class RectangleJsonConverter : JsonConverter<Rectangle?>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Rectangle) ||
               typeToConvert == typeof(Rectangle?);
    }

    public override Rectangle? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        else if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("JSON string expected");
        }

        string value = reader.GetString();

        if (string.IsNullOrEmpty(value))
        {
            throw new JsonException($"Unexpected empty or null string");
        }

        string separator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
        string[] xywh = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        if (xywh.Length != 4)
        {
            throw new JsonException($"Invalid format, expected 'x{separator}y{separator}w{separator}h', got '{value}'");
        }

        if (!int.TryParse(xywh[0], CultureInfo.InvariantCulture, out int x))
        {
            throw new JsonException($"Invalid format, expected float, got '{xywh[0]}'");
        }

        if (!int.TryParse(xywh[1], CultureInfo.InvariantCulture, out int y))
        {
            throw new JsonException($"Invalid format, expected float, got '{xywh[1]}'");
        }

        if (!int.TryParse(xywh[2], CultureInfo.InvariantCulture, out int width))
        {
            throw new JsonException($"Invalid format, expected float, got '{xywh[2]}'");
        }

        if (!int.TryParse(xywh[3], CultureInfo.InvariantCulture, out int height))
        {
            throw new JsonException($"Invalid format, expected float, got '{xywh[2]}'");
        }

        return new Rectangle(x, y, width, height);
    }

    public override void Write(Utf8JsonWriter writer, Rectangle? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (value is Rectangle rect)
        {
            string format = "{1}{0}{2}{0}{3}{0}{4}";
            string separator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
            string output = string.Format(CultureInfo.InvariantCulture, format, separator, rect.X, rect.Y, rect.Width, rect.Height);
            writer.WriteStringValue(output);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
