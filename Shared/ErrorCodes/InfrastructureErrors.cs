namespace MoneyTracker.ErrorCodes;

/// <summary>Ошибки приложения.</summary>
public static partial class Errors
{
    /// <summary>Ошибки инфраструктурного слоя приложения.</summary>
    public static class Infrastructure
    {
        /// <summary>Ошибки, связанные с работой с данными (БД, ORM и т.д.).</summary>
        public static class Data
        {
            /// <summary>Указанный провайдер данных не поддерживается системой.</summary>
            /// <remarks>
            /// Пример: попытка использовать NHibernate, который еще не поддерживается.
            /// </remarks>
            public const string UnsupportedDataProvider = "error.infrastructure.data.unsupported_data_provider";

            /// <summary>Отсутствует обязательная конфигурация для подключения к базе данных.</summary>
            /// <remarks>
            /// Проверьте наличие секции 'Database' в appsettings.json.
            /// </remarks>
            public const string MissingConfiguration = "error.infrastructure.data.database_configuration_is_missing";

            /// <summary>Указан неподдерживаемый тип ORM.</summary>
            /// <remarks>
            /// Допустимые значения: 'EntityFramework', 'Dapper', 'NHibernate'.
            /// </remarks>
            public const string InvalidOrmType = "error.infrastructure.data.invalid_orm_type";

            /// <summary>Некорректный провайдер базы данных.</summary>
            /// <remarks>
            /// Проверьте, что провайдер зарегистрирован в DI-контейнере.
            /// </remarks>
            public const string InvalidDbProvider = "error.infrastructure.data.invalid_db_provider";

            /// <summary>Выбранная ORM не поддерживает требуемую функциональность.</summary>
            /// <remarks>
            /// Например, попытка использовать NoSQL-запросы в SQL-ориентированной ORM.
            /// </remarks>
            public const string UnsupportedOrm = "error.infrastructure.data.unsupported_orm";
        }
    }
}