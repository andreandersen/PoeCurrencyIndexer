using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PoeCurrencyIndexer.Indexer.Fetch.Models
{
    public class NoteJsonConverter : JsonConverter<Note>
    {
        public override Note? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
                return (Note)reader.GetString();
            
            return Note.Empty;                
        }

        public override void Write(Utf8JsonWriter writer, Note value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Original);
    }

}
