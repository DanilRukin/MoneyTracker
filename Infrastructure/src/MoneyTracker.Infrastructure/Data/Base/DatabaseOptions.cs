using System.Text.Json.Serialization;

namespace MoneyTracker.Infrastructure.Data.Base
{
    /// <summary>
    /// Опции БД
    /// </summary>
    public class DatabaseOptions
    {
        /// <summary>
        /// Тип СУБД
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProviderTypes Provider { get; set; }

        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Флаг автоматической миграции
        /// </summary>
        public bool AutoMigrate { get; set; } = true;

        /// <summary>
        /// Указывает, заполнять ли БД тестовыми данными
        /// </summary>
        public bool SeedTestData { get; set; } = false;

        /// <summary>
        /// Тип используемой ОРМ
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrmTypes OrmType { get; set; }
    }
}
