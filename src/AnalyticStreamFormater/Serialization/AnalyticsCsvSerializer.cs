using System.Globalization;
using System.Text;
using AnalyticStreamFormater.Domain.Models;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace AnalyticStreamFormater.Serialization
{
    internal class AnalyticsCsvSerializer
    {
        private readonly CsvConfiguration _config;

        public AnalyticsCsvSerializer(string delimiter = ",")
        {
            _config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter
            };
        }

        public string GetHeaders(AnalyticsDto analytics)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, _config))
                {
                    csvWriter.Context.RegisterClassMap<AnalyticsDtoMap>();
                    csvWriter.WriteHeader<AnalyticsDto>();
                    foreach (var header in analytics.Values.Keys)
                    {
                        csvWriter.WriteField(header);
                    }
                    csvWriter.NextRecord();
                }
                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }

        public string GetRow(AnalyticsDto analytics)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter, _config))
                {
                    csvWriter.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "o" };
                    csvWriter.Context.TypeConverterCache.RemoveConverter<DateTime>();
                    csvWriter.Context.TypeConverterCache.AddConverter<DateTime>(new UniversalTimeConverter());
                    csvWriter.WriteRecord(analytics);

                    foreach (var value in analytics.Values)
                    {
                        csvWriter.WriteField(value.Value);
                    }

                    csvWriter.NextRecord();
                }

                return Encoding.Default.GetString(memoryStream.ToArray());
            }
        }

        class UniversalTimeConverter : DefaultTypeConverter
        {
            public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
            {
                return base.ConvertFromString(text, row, memberMapData);
            }

            public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                return base.ConvertToString(((DateTime)value).ToUniversalTime(), row, memberMapData);
            }
        }

        class AnalyticsDtoMap : ClassMap<AnalyticsDto>
        {
            public AnalyticsDtoMap()
            {
                AutoMap(CultureInfo.InvariantCulture);
                Map(m => m.Entity.Type).Name("EntityType");
                Map(m => m.Schema.Id).Name("SchemaId");
                Map(m => m.Schema.Version.Major).Name("SchemaMajorVersion");
                Map(m => m.Schema.Version.Minor).Name("SchemaMinorVersion");
            }
        }
    }
}
