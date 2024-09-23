// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Modifiers;
using Aristurtle.ParticleEngine.Modifiers.Containers;
using Aristurtle.ParticleEngine.Modifiers.Interpolators;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ModifierJsonConverter : JsonConverter<Modifier>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Modifier);
    }

    public override Modifier Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Expected start of object when deserialziing {typeToConvert.GetType()}.");
        }

        string name = null;
        float frequency = 0.0f;
        string type = null;
        Modifier result = null;

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
                case nameof(Modifier.Name):
                    name = reader.GetString();
                    break;

                case nameof(Modifier.Frequency):
                    frequency = reader.GetSingle();
                    break;

                case nameof(Type):
                    type = reader.GetString();
                    break;

                default:
                    if (result is null)
                    {
                        result = CreateModifier(type, name, frequency);
                    }
                    ReadModifierProperties(ref reader, result, propertyName, options);
                    break;
            }
        }

        if (result == null)
        {
            result = CreateModifier(type, name, frequency);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Modifier value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartObject();

        writer.WriteString(nameof(Modifier.Name), value.Name);
        writer.WriteNumber(nameof(Modifier.Frequency), value.Frequency);

        switch (value)
        {
            case AgeModifier age:
                writer.WriteString(nameof(Type), nameof(AgeModifier));

                writer.WritePropertyName(nameof(AgeModifier.Interpolators));
                JsonSerializer.Serialize(writer, age.Interpolators, options);
                break;

            case CircleContainerModifier circle:
                writer.WriteString(nameof(Type), nameof(CircleContainerModifier));
                writer.WriteNumber(nameof(CircleContainerModifier.Radius), circle.Radius);
                writer.WriteBoolean(nameof(CircleContainerModifier.Inside), circle.Inside);
                writer.WriteNumber(nameof(CircleContainerModifier.RestitutionCoefficient), circle.RestitutionCoefficient);
                break;

            case DragModifier drag:
                writer.WriteString(nameof(Type), nameof(DragModifier));
                writer.WriteNumber(nameof(DragModifier.DragCoefficient), drag.DragCoefficient);
                writer.WriteNumber(nameof(DragModifier.Density), drag.Density);
                break;

            case LinearGravityModifier linearGravity:
                writer.WriteString(nameof(Type), nameof(LinearGravityModifier));

                writer.WritePropertyName(nameof(LinearGravityModifier.Direction));
                JsonSerializer.Serialize(writer, linearGravity.Direction, options);

                writer.WriteNumber(nameof(LinearGravityModifier.Strength), linearGravity.Strength);
                break;

            case OpacityFastFadeModifier opacity:
                writer.WriteString(nameof(Type), nameof(OpacityFastFadeModifier));
                break;

            case RectangleContainerModifier rectangle:
                writer.WriteString(nameof(Type), nameof(RectangleContainerModifier));
                writer.WriteNumber(nameof(RectangleContainerModifier.Width), rectangle.Width);
                writer.WriteNumber(nameof(RectangleContainerModifier.Height), rectangle.Height);
                writer.WriteNumber(nameof(RectangleContainerModifier.RestitutionCoefficient), rectangle.RestitutionCoefficient);
                break;

            case RectangleLoopContainerModifier rectangleLoop:
                writer.WriteString(nameof(Type), nameof(RectangleLoopContainerModifier));
                writer.WriteNumber(nameof(RectangleLoopContainerModifier.Width), rectangleLoop.Width);
                writer.WriteNumber(nameof(RectangleLoopContainerModifier.Height), rectangleLoop.Height);
                break;

            case RotationModifier rotation:
                writer.WriteString(nameof(Type), nameof(RotationModifier));
                writer.WriteNumber(nameof(RotationModifier.RotationRate), rotation.RotationRate);
                break;

            case VelocityColorModifier velocityColor:
                writer.WriteString(nameof(Type), nameof(VelocityColorModifier));

                writer.WritePropertyName(nameof(VelocityColorModifier.StationaryColor));
                JsonSerializer.Serialize(writer, velocityColor.StationaryColor, options);

                writer.WritePropertyName(nameof(VelocityColorModifier.VelocityColor));
                JsonSerializer.Serialize(writer, velocityColor.VelocityColor, options);

                writer.WriteNumber(nameof(VelocityColorModifier.VelocityThreshold), velocityColor.VelocityThreshold);
                break;

            case VelocityModifier velocity:
                writer.WriteString(nameof(Type), nameof(VelocityModifier));
                writer.WriteNumber(nameof(VelocityModifier.VelocityThreshold), velocity.VelocityThreshold);

                writer.WritePropertyName(nameof(VelocityModifier.Interpolators));
                JsonSerializer.Serialize(writer, velocity.Interpolators, options);
                break;

            case VortexModifier vortex:
                writer.WriteString(nameof(Type), nameof(VortexModifier));

                writer.WritePropertyName(nameof(VortexModifier.Position));
                JsonSerializer.Serialize(writer, vortex.Position, options);

                writer.WriteNumber(nameof(VortexModifier.Mass), vortex.Mass);
                writer.WriteNumber(nameof(VortexModifier.MaxSpeed), vortex.MaxSpeed);
                break;

            default:
                throw new JsonException($"Unknown modifier type: {value.GetType()}");
        }

        writer.WriteEndObject();
    }

    private static Modifier CreateModifier(string type, string name, float frequency)
    {
        return type switch
        {
            nameof(AgeModifier) => new AgeModifier { Name = name, Frequency = frequency },
            nameof(CircleContainerModifier) => new CircleContainerModifier { Name = name, Frequency = frequency },
            nameof(DragModifier) => new DragModifier { Name = name, Frequency = frequency },
            nameof(LinearGravityModifier) => new LinearGravityModifier { Name = name, Frequency = frequency },
            nameof(OpacityFastFadeModifier) => new OpacityFastFadeModifier { Name = name, Frequency = frequency },
            nameof(RectangleContainerModifier) => new RectangleContainerModifier { Name = name, Frequency = frequency },
            nameof(RectangleLoopContainerModifier) => new RectangleLoopContainerModifier { Name = name, Frequency = frequency },
            nameof(RotationModifier) => new RotationModifier { Name = name, Frequency = frequency },
            nameof(VelocityColorModifier) => new VelocityColorModifier { Name = name, Frequency = frequency },
            nameof(VelocityModifier) => new VelocityModifier { Name = name, Frequency = frequency },
            nameof(VortexModifier) => new VortexModifier { Name = name, Frequency = frequency },
            _ => throw new JsonException($"Unknown modifier type: {type}")
        };
    }

    private static void ReadModifierProperties(ref Utf8JsonReader reader, Modifier modifier, string propertyName, JsonSerializerOptions options)
    {
        switch (modifier)
        {
            case AgeModifier age:
                if (propertyName == nameof(AgeModifier.Interpolators))
                {
                    age.Interpolators = JsonSerializer.Deserialize<List<Interpolator>>(ref reader, options);
                }
                break;

            case CircleContainerModifier circle:
                switch (propertyName)
                {
                    case nameof(CircleContainerModifier.Radius):
                        circle.Radius = reader.GetSingle();
                        break;

                    case nameof(CircleContainerModifier.Inside):
                        circle.Inside = reader.GetBoolean();
                        break;

                    case nameof(CircleContainerModifier.RestitutionCoefficient):
                        circle.RestitutionCoefficient = reader.GetSingle();
                        break;
                }
                break;

            case DragModifier drag:
                switch (propertyName)
                {
                    case nameof(DragModifier.DragCoefficient):
                        drag.DragCoefficient = reader.GetSingle();
                        break;

                    case nameof(DragModifier.Density):
                        drag.Density = reader.GetSingle();
                        break;
                }
                break;

            case LinearGravityModifier linearGravity:
                switch (propertyName)
                {
                    case nameof(LinearGravityModifier.Direction):
                        linearGravity.Direction = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;

                    case nameof(LinearGravityModifier.Strength):
                        linearGravity.Strength = reader.GetSingle();
                        break;
                }
                break;

            case RectangleContainerModifier rectangle:
                switch (propertyName)
                {
                    case nameof(RectangleContainerModifier.Width):
                        rectangle.Width = reader.GetInt32();
                        break;

                    case nameof(RectangleContainerModifier.Height):
                        rectangle.Height = reader.GetInt32();
                        break;

                    case nameof(RectangleContainerModifier.RestitutionCoefficient):
                        rectangle.RestitutionCoefficient = reader.GetSingle();
                        break;
                }
                break;

            case RectangleLoopContainerModifier rectangleLoop:
                switch (propertyName)
                {
                    case nameof(RectangleLoopContainerModifier.Width):
                        rectangleLoop.Width = reader.GetInt32();
                        break;

                    case nameof(RectangleLoopContainerModifier.Height):
                        rectangleLoop.Height = reader.GetInt32();
                        break;
                }
                break;

            case RotationModifier rotation:
                if (propertyName == nameof(RotationModifier.RotationRate))
                {
                    rotation.RotationRate = reader.GetSingle();
                }
                break;

            case VelocityColorModifier velocityColor:
                switch (propertyName)
                {
                    case nameof(VelocityColorModifier.StationaryColor):
                        velocityColor.StationaryColor = JsonSerializer.Deserialize<Vector3>(ref reader, options);
                        break;

                    case nameof(VelocityColorModifier.VelocityColor):
                        velocityColor.VelocityColor = JsonSerializer.Deserialize<Vector3>(ref reader, options);
                        break;

                    case nameof(VelocityColorModifier.VelocityThreshold):
                        velocityColor.VelocityThreshold = reader.GetSingle();
                        break;
                }
                break;

            case VelocityModifier velocity:
                switch (propertyName)
                {
                    case nameof(VelocityModifier.VelocityThreshold):
                        velocity.VelocityThreshold = reader.GetSingle();
                        break;

                    case nameof(VelocityModifier.Interpolators):
                        velocity.Interpolators = JsonSerializer.Deserialize<List<Interpolator>>(ref reader, options);
                        break;
                }
                break;

            case VortexModifier vortex:
                switch (propertyName)
                {
                    case nameof(VortexModifier.Position):
                        vortex.Position = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;

                    case nameof(VortexModifier.Mass):
                        vortex.Mass = reader.GetSingle();
                        break;

                    case nameof(VortexModifier.MaxSpeed):
                        vortex.MaxSpeed = reader.GetSingle();
                        break;
                }
                break;

            default:
                throw new JsonException($"Unknown modifier type: {modifier.GetType()}");
        }
    }

}
