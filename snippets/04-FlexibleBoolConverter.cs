/// <summary>
///  This is a custom JSON converter class using System.Text.Json to handle flexible formats of booleans during deserialization.
///  It overrides the Read and Write methods from JsonConverter<bool> to support flexible boolean parsing.
/// </summary>
public class FlexibleBoolConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {

        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                var str = reader.GetString()?.Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(str))
                    return false;

                return str switch
                {
                    "true" => true,
                    "false" => false,
                    "1" => true,
                    "0" => false,
                    "yes" => true,
                    "no" => false,
                    "y" or "on" => true,
                    "n" or "off" => false,
                    _ => throw new JsonException($"Cannot convert string '{str}' to boolean.")
                };

            case JsonTokenType.Number:
                if (reader.TryGetInt32(out int number))
                    return number != 0;
                throw new JsonException($"Only integers 0 or 1 can be used to represent booleans, but got: {reader.GetDouble()}");

            case JsonTokenType.True:
                return true;

            case JsonTokenType.False:
                return false;

            default:
                throw new JsonException($"Unexpected token {reader.TokenType} when parsing boolean.");                                   
        }
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
