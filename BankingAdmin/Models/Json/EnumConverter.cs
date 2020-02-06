using System;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BankingAdmin.Models.Json
{
    public class EnumConverter<T> : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, 
            JsonSerializerOptions options)
        {
            return (T)Enum.Parse(typeof(T), reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, T value, 
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }

    }
}
