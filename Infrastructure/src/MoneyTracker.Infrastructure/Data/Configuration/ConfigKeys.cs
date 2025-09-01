namespace MoneyTracker.Infrastructure.Data.Configuration
{
    /// <summary>
    /// Ключи конфигурации системы DatabaseProvider
    /// </summary>
    public static class ConfigKeys
    {
        private static string _delimeter = ":";

        /// <summary>Ключ конфигурации Database</summary>
        public static class Database
        {
            /// <summary>Получает строковое представление ключа конфигурации Database</summary>
            public static string Get() => nameof(Database);

            /// <summary>Ключ конфигурации Provider</summary>
            public static class Provider
            {
                /// <summary>Получает строковое представление ключа конфигурации Provider</summary>
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(Provider)}";
            }

            /// <summary>Ключ конфигурации OrmType</summary>
            public static class OrmType
            {
                /// <summary>Получает строковое представление ключа конфигурации OrmType</summary>
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(OrmType)}";
            }

            /// <summary>Ключ конфигурации ConnectionString</summary>
            public static class ConnectionString
            {
                /// <summary>Получает строковое представление ключа конфигурации ConnectionString</summary>
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(ConnectionString)}";
            }

            /// <summary>Ключ конфигурации AutoMigrate</summary>
            public static class AutoMigrate
            {
                /// <summary>Получает строковое представление ключа конфигурации AutoMigrate</summary>
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(AutoMigrate)}";
            }

            /// <summary>Ключ конфигурации SeedTestData</summary>
            public static class SeedTestData
            {
                /// <summary>Получает строковое представление ключа конфигурации SeedTestData</summary>
                public static string Get() => $"{Database.Get()}{_delimeter}{nameof(SeedTestData)}";
            }
        }
    }
}
