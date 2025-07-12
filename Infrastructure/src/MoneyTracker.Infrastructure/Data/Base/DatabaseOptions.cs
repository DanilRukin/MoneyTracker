using System.Text.Json.Serialization;

namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Опции БД
    /// </summary>
    public class DatabaseOptions
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProviderTypes Provider { get; set; }
        public string ConnectionString { get; set; }
        public bool AutoMigrate { get; set; } = true;
        public bool SeedTestData { get; set; } = false;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrmTypes OrmType { get; set; }
    }
}
