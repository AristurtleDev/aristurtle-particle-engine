// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Profiles;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ProfileJsonConverter : JsonConverter<Profile>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Profile);
    }

    public override Profile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException($"Expected start of object when deserialziing {typeToConvert.GetType()}.");
        }

        string type = null;
        Profile result = null;

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
                case nameof(Type):
                    type = reader.GetString();
                    break;

                default:
                    if (result is null)
                    {
                        result = CreateProfile(type);
                    }
                    ReadProfileProperties(ref reader, result, propertyName, options);
                    break;
            }
        }

        if (result == null)
        {
            result = CreateProfile(type);
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, Profile value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteStartObject();

        switch (value)
        {
            case BoxFillProfile boxFill:
                writer.WriteString(nameof(Type), nameof(BoxFillProfile));
                writer.WriteNumber(nameof(BoxFillProfile.Width), boxFill.Width);
                writer.WriteNumber(nameof(BoxFillProfile.Height), boxFill.Height);
                break;

            case BoxProfile box:
                writer.WriteString(nameof(Type), nameof(BoxProfile));
                writer.WriteNumber(nameof(BoxProfile.Width), box.Width);
                writer.WriteNumber(nameof(BoxProfile.Height), box.Height);
                break;

            case BoxUniformProfile boxUniform:
                writer.WriteString(nameof(Type), nameof(BoxUniformProfile));
                writer.WriteNumber(nameof(BoxUniformProfile.Width), boxUniform.Width);
                writer.WriteNumber(nameof(BoxUniformProfile.Height), boxUniform.Height);
                break;

            case CircleProfile circle:
                writer.WriteString(nameof(Type), nameof(CircleProfile));
                writer.WriteNumber(nameof(CircleProfile.Radius), circle.Radius);
                writer.WritePropertyName(nameof(CircleProfile.Radiate));
                JsonSerializer.Serialize(writer, circle.Radiate, options);
                break;

            case LineProfile line:
                writer.WriteString(nameof(Type), nameof(LineProfile));
                writer.WritePropertyName(nameof(LineProfile.Axis));
                JsonSerializer.Serialize(writer, line.Axis, options);
                writer.WriteNumber(nameof(LineProfile.Length), line.Length);
                break;

            case LineUniformProfile lineUniform:
                writer.WriteString(nameof(Type), nameof(LineUniformProfile));
                writer.WritePropertyName(nameof(LineUniformProfile.Axis));
                JsonSerializer.Serialize(writer, lineUniform.Axis, options);
                writer.WriteNumber(nameof(LineUniformProfile.Length), lineUniform.Length);
                writer.WritePropertyName(nameof(LineUniformProfile.PerpendicularDirection));
                JsonSerializer.Serialize(writer, lineUniform.PerpendicularDirection, options);
                break;

            case PointProfile point:
                writer.WriteString(nameof(Type), nameof(PointProfile));
                break;

            case RingProfile ring:
                writer.WriteString(nameof(Type), nameof(RingProfile));
                writer.WriteNumber(nameof(RingProfile.Radius), ring.Radius);
                writer.WritePropertyName(nameof(RingProfile.Radiate));
                JsonSerializer.Serialize(writer, ring.Radiate, options);
                break;

            case SprayProfile spray:
                writer.WriteString(nameof(Type), nameof(LineProfile));
                writer.WritePropertyName(nameof(SprayProfile.Direction));
                JsonSerializer.Serialize(writer, spray.Direction, options);
                writer.WriteNumber(nameof(SprayProfile.Spread), spray.Spread);
                break;

            default:
                throw new JsonException($"Unexpected {nameof(Profile)}: {value.GetType()}");
        }

        writer.WriteEndObject();
    }

    private static Profile CreateProfile(string type)
    {
        return type switch
        {
            nameof(BoxFillProfile) => new BoxFillProfile(),
            nameof(BoxProfile) => new BoxProfile(),
            nameof(BoxUniformProfile) => new BoxUniformProfile(),
            nameof(CircleProfile) => new CircleProfile(),
            nameof(LineProfile) => new LineProfile(),
            nameof(LineUniformProfile) => new LineUniformProfile(),
            nameof(PointProfile) => new PointProfile(),
            nameof(RingProfile) => new RingProfile(),
            nameof(SprayProfile) => new SprayProfile(),
            _ => throw new JsonException($"Unknown {nameof(Profile)} type: {type}")
        };
    }

    private static void ReadProfileProperties(ref Utf8JsonReader reader, Profile profile, string propertyName, JsonSerializerOptions options)
    {
        switch (profile)
        {
            case BoxFillProfile boxFill:
                switch (propertyName)
                {
                    case nameof(BoxFillProfile.Width):
                        boxFill.Width = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(BoxFillProfile.Height):
                        boxFill.Height = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case BoxProfile box:
                switch (propertyName)
                {
                    case nameof(BoxProfile.Width):
                        box.Width = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(BoxProfile.Height):
                        box.Height = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case BoxUniformProfile boxUniform:
                switch (propertyName)
                {
                    case nameof(BoxUniformProfile.Width):
                        boxUniform.Width = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(BoxUniformProfile.Height):
                        boxUniform.Height = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case CircleProfile circle:
                switch (propertyName)
                {
                    case nameof(CircleProfile.Radius):
                        circle.Radius = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(CircleProfile.Radiate):
                        circle.Radiate = JsonSerializer.Deserialize<CircleRadiation>(ref reader, options);
                        break;
                }
                break;

            case LineProfile line:
                switch (propertyName)
                {
                    case nameof(LineProfile.Axis):
                        line.Axis = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;

                    case nameof(LineProfile.Length):
                        line.Length = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            case LineUniformProfile lineUniform:
                switch (propertyName)
                {
                    case nameof(LineUniformProfile.Axis):
                        lineUniform.Axis = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;

                    case nameof(LineUniformProfile.Length):
                        lineUniform.Length = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(LineUniformProfile.PerpendicularDirection):
                        lineUniform.PerpendicularDirection = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;
                }
                break;

            case PointProfile point:
                //  Nothing to read for point
                break;

            case RingProfile ring:
                switch (propertyName)
                {
                    case nameof(RingProfile.Radius):
                        ring.Radius = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;

                    case nameof(RingProfile.Radiate):
                        ring.Radiate = JsonSerializer.Deserialize<CircleRadiation>(ref reader, options);
                        break;
                }
                break;

            case SprayProfile spray:
                switch (propertyName)
                {
                    case nameof(SprayProfile.Direction):
                        spray.Direction = JsonSerializer.Deserialize<Vector2>(ref reader, options);
                        break;

                    case nameof(SprayProfile.Spread):
                        spray.Spread = JsonSerializer.Deserialize<float>(ref reader, options);
                        break;
                }
                break;

            default:
                throw new JsonException($"Unknown {nameof(Profile)} type: {profile.GetType()}");
        }
    }

}
