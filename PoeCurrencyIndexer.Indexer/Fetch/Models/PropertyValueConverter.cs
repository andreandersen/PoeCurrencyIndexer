using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class PropertyValueConverter : JsonConverter<PropertyValue[]>
    {
        public override PropertyValue[]? Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException("Expected Start of Array");

            var values = new List<PropertyValue>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    break;

                // string value at index 0
                _ = reader.Read();
                var val = reader.GetString();

                // int value at index 1
                _ = reader.Read();
                var type = reader.GetInt32();

                var v = new PropertyValue(val!, type);
                values.Add(v);
                
                // End of Array
                _ = reader.Read();
            }

            return values.ToArray();
        }

        public override void Write(
            Utf8JsonWriter writer, PropertyValue[] value, JsonSerializerOptions options) =>
            throw new NotImplementedException();
    }
}
