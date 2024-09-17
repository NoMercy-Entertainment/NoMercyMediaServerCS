using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace NoMercy.NmSystem;

public static class JsonHelper
{
    public static readonly JsonSerializerSettings Settings = new()
    {
        // General settings
        Formatting = Formatting.Indented, // Pretty print JSON
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore, // Ignore $id and $ref properties
        DateParseHandling = DateParseHandling.None, // Treat dates as strings
        FloatParseHandling = FloatParseHandling.Double, // Parse floats as double

        // Reference handling
        PreserveReferencesHandling = PreserveReferencesHandling.None, // Do not use $id and $ref
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Ignore reference loops

        // Null and default value handling
        NullValueHandling = NullValueHandling.Include, // Include null values
        DefaultValueHandling = DefaultValueHandling.Include, // Include default values

        // Error handling
        Error = (_, ev) => { ev.ErrorContext.Handled = true; }, // Handle errors silently

        // Type handling
        TypeNameHandling = TypeNameHandling.None, // Do not include type names

        // Missing member handling
        MissingMemberHandling = MissingMemberHandling.Ignore, // Ignore missing members

        // Converters
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }, // Convert dates to ISO format
            new ParseNumbersAsInt32Converter(), // Custom converter for parsing numbers as int
            new StringEnumConverter(), // Convert enums to strings
            new DoubleConverter() // Custom converter for double values
        }
    };

    public static T? FromJson<T>(this string json)
    {
        return JsonConvert.DeserializeObject<T>(json, Settings);
    }

    public static string ToJson<T>(this T self)
    {
        return JsonConvert.SerializeObject(self, Settings);
    }
}

public class ParseNumbersAsInt32Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(long) || objectType == typeof(object);
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is int intValue)
        {
            writer.WriteValue(intValue);
        }
        else
        {
            // Fallback to default serialization for other types
            // serializer.Serialize(writer, value);
        }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        return reader.Value is long
            ? Convert.ToInt64(reader.Value ?? 0)
            : reader.Value;
    }
}

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