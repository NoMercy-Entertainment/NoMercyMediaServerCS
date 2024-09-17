using Newtonsoft.Json;

namespace NoMercy.NmSystem;
public class DoubleConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(double) || objectType == typeof(double?);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is double doubleValue)
        {
            if (double.IsInfinity(doubleValue) || double.IsNaN(doubleValue))
                writer.WriteNull();
            else
                writer.WriteValue(doubleValue);
        }
        else
        {
            writer.WriteNull();
        }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;

        if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            return Convert.ToDouble(reader.Value);

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing double.");
    }
}