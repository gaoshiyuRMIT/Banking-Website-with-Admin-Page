using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;

namespace BankingAdmin.Models.Json 
{
    public class BankingDateTimeConverter : JsonConverter<DateTime>
    {
        private const string format = "dd/MM/yyyy hh:mm:ss tt";
        private readonly CultureInfo culture = CultureInfo.InvariantCulture;
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, 
            JsonSerializerOptions options)
        {
            return DateTime.SpecifyKind(
                DateTime.ParseExact(reader.GetString(), format, culture), 
                DateTimeKind.Utc);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, 
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format));
        }
    }

}