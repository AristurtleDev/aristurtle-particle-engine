// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class InterpolatorJsonConverter : JsonConverter<Interpolator>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Interpolator);
    }

    public override Interpolator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Expected start of object when deserialziing {typeToConvert.GetType()}.");
        }

        string name = null;
        string type = null;
        Interpolator result = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name");
            }

            string propertyName = reader.GetString();
            reader.Read();

            switch (propertyName)
            {
                case nameof(Interpolator.Name):
                    name = reader.GetString();
                    break;

                case nameof(Type):
                    type = reader.GetString();
                    break;

                default:
                    if (result is null)
                    {
                        result = CreateInterpolator(type, name);
                    }
                    ReadInterpolatorProperties(ref reader, result, propertyName, options);
                    break;
            }
        }

        if (result == null)
        {
            result = CreateInterpolator(type, name);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Interpolator value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartObject();

        writer.WriteString(nameof(Interpolator.Name), value.Name);

        switch (value)
        {
            case ColorInterpolator color:
                writer.WriteString(nameof(Type), nameof(ColorInterpolator));
                writer.WritePropertyName(nameof(ColorInterpolator.StartValue));
                JsonSerializer.Serialize(writer, color.StartValue, options);
                writer.WritePropertyName(nameof(ColorInterpolator.EndValue));
                JsonSerializer.Serialize(writer, color.EndValue, options);
                break;

            case HueInterpolator hue:
                writer.WriteString(nameof(Type), nameof(HueInterpolator));
                writer.WritePropertyName(nameof(HueInterpolator.StartValue));
                JsonSerializer.Serialize(writer, hue.StartValue, options);
                writer.WritePropertyName(nameof(HueInterpolator.EndValue));
                JsonSerializer.Serialize(writer, hue.EndValue, options);
                break;

            case OpacityInterpolator opacity:
                writer.WriteString(nameof(Type), nameof(OpacityInterpolator));
                writer.WritePropertyName(nameof(OpacityInterpolator.StartValue));
                JsonSerializer.Serialize(writer, opacity.StartValue, options);
                writer.WritePropertyName(nameof(OpacityInterpolator.EndValue));
                JsonSerializer.Serialize(writer, opacity.EndValue, options);
                break;

            case RotationInterpolator rotation:
                writer.WriteString(nameof(Type), nameof(RotationInterpolator));
                writer.WritePropertyName(nameof(RotationInterpolator.StartValue));
                JsonSerializer.Serialize(writer, rotation.StartValue, options);
                writer.WritePropertyName(nameof(RotationInterpolator.EndValue));
                JsonSerializer.Serialize(writer, rotation.EndValue, options);
                break;

            case ScaleInterpolator scale:
                writer.WriteString(nameof(Type), nameof(ScaleInterpolator));
                writer.WritePropertyName(nameof(ScaleInterpolator.StartValue));
                JsonSerializer.Serialize(writer, scale.StartValue, options);
                writer.WritePropertyName(nameof(ScaleInterpolator.EndValue));
                JsonSerializer.Serialize(writer, scale.EndValue, options);
                break;

            case VelocityInterpolator velocity:
                writer.WriteString(nameof(Type), nameof(VelocityInterpolator));
                writer.WritePropertyName(nameof(VelocityInterpolator.StartValue));
                JsonSerializer.Serialize(writer, velocity.StartValue, options);
                writer.WritePropertyName(nameof(VelocityInterpolator.EndValue));
                JsonSerializer.Serialize(writer, velocity.EndValue, options);
                break;

            default:
                throw new JsonException($"Unexpected {nameof(Interpolator)}: {value.GetType()}");
        }

        writer.WriteEndObject();
    }

    private static Interpolator CreateInterpolator(string type, string name)
    {
        return type switch
        {
            nameof(ColorInterpolator) => new ColorInterpolator { Name = name },
            nameof(HueInterpolator) => new HueInterpolator { Name = name },
            nameof(OpacityInterpolator) => new OpacityInterpolator { Name = name },
            nameof(RotationInterpolator) => new RotationInterpolator { Name = name },
            nameof(ScaleInterpolator) => new ScaleInterpolator { Name = name },
            nameof(VelocityInterpolator) => new VelocityInterpolator { Name = name },
            _ => throw new JsonException($"Unknown {nameof(Interpolator)} type: {type}")
        };
    }

    private static void ReadInterpolatorProperties(ref Utf8JsonReader reader, Interpolator interpolator, string propertyName, JsonSerializerOptions options)
    {
        switch (interpolator)
        {
            case ColorInterpolator color:
                switch (propertyName)
                {
                    case nameof(ColorInterpolator.StartValue):
                        color.StartValue = JsonSerializer.Deserialize<Vector3>(ref reader, options);
                        break;

                    case nameof(ColorInterpolator.EndValue):
                        color.EndValue = JsonSerializer.Deserialize<Vector3>(ref reader, options);
                        break;
                }
                break;

            case HueInterpolator hue:
                switch (propertyName)
                {
                    case nameof(HueInterpolator.StartValue):
                        hue.StartValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(HueInterpolator.EndValue):
                        hue.EndValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case OpacityInterpolator opacity:
                switch (propertyName)
                {
                    case nameof(OpacityInterpolator.StartValue):
                        opacity.StartValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(OpacityInterpolator.EndValue):
                        opacity.EndValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case RotationInterpolator rotation:
                switch (propertyName)
                {
                    case nameof(RotationInterpolator.StartValue):
                        rotation.StartValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(RotationInterpolator.EndValue):
                        rotation.EndValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case ScaleInterpolator scale:
                switch (propertyName)
                {
                    case nameof(ScaleInterpolator.StartValue):
                        scale.StartValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(ScaleInterpolator.EndValue):
                        scale.EndValue = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case VelocityInterpolator velocity:
                switch (propertyName)
                {
                    case nameof(VelocityInterpolator.StartValue):
                        velocity.StartValue = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;

                    case nameof(VelocityInterpolator.EndValue):
                        velocity.EndValue = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;
                }
                break;

            default:
                throw new JsonException($"Unknown {nameof(Interpolator)} type: {interpolator.GetType()}");
        }
    }

}
