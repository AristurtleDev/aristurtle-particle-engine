// Released under The Unlicense.
// See LICENSE file in the project root for full license information.
// License information can also be found at https://unlicense.org/.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Aristurtle.ParticleEngine.Modifiers;

namespace Aristurtle.ParticleEngine.Serialization.Json;

internal sealed class ModifierExecutionStrategyConverter : JsonConverter<ModifierExecutionStrategy>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ModifierExecutionStrategy);
    }

    public override ModifierExecutionStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected string value");
        }

        string strategyType = reader.GetString();
        ModifierExecutionStrategy result = strategyType switch
        {
            nameof(ModifierExecutionStrategy.Serial) => ModifierExecutionStrategy.Serial,
            nameof(ModifierExecutionStrategy.Parallel) => ModifierExecutionStrategy.Parallel,
            _ => throw new JsonException($"Unknown {nameof(ModifierExecutionStrategy)} type: '{strategyType}'")
        };

        return result;
    }

    public override void Write(Utf8JsonWriter writer, ModifierExecutionStrategy value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);

        switch(value)
        {
            case ModifierExecutionStrategy.SerialModifierExecutionStrategy:
                writer.WriteStringValue(nameof(ModifierExecutionStrategy.Serial));
                break;

            case ModifierExecutionStrategy.ParallelModifierExecutionStrategy:
                writer.WriteStringValue(nameof(ModifierExecutionStrategy.Parallel));
                break;
        }
    }

    private static void ThrowInvalidFormatException(string input)
    {
        string format = "Invalid format for {0}.  Expected 'Serial' or 'Parallel', got '{1}'";
        string type = nameof(ModifierExecutionStrategy);
        ThrowJsonException(format, type, input);
    }

    private static void ThrowJsonException(string format, params object[] args)
    {
        throw new JsonException(string.Format(CultureInfo.InvariantCulture, format, args));
    }
}
