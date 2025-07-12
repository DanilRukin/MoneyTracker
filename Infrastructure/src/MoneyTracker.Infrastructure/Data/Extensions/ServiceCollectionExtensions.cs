using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Data.Base;
using MoneyTracker.Infrastructure.Data.Configuration;
using MoneyTracker.Infrastructure.Data.Providers;

namespace MoneyTracker.Infrastructure.Data.Extensions
{
    /// <summary>
    /// Методы расширений <see cref="IServiceCollection"/> для добавления поставщиков БД
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет провайдер БД
        /// </summary>
        /// <param name="services">Коллекция сервисов приложения</param>
        /// <param name="configure">Метод настройки опций БД</param>
        /// <exception cref="NotSupportedException"></exception>
        public static IServiceCollection AddDatabaseProvider(
            this IServiceCollection services,
            Action<DatabaseOptions> configure)
        {
            DatabaseOptions options = new();
            configure(options);

            services.AddSingleton(options);
            switch (options.OrmType)
            {
                case OrmTypes.EntityFrameworkCore:
                    services.AddSingleton<IDatabaseProvider, EfCoreDatabaseProvider>();
                    break;
                case OrmTypes.NHibernate:
                    services.AddSingleton<IDatabaseProvider, NHibernateProvider>();
                    break;
                case OrmTypes.Dapper:
                    services.AddSingleton<IDatabaseProvider, DapperProvider>();
                    break;
                default:
                    throw new NotSupportedException($"Unsupported ORM: {options.OrmType}");
            }

            return services;
        }

        /// <summary>
        /// Добавляет провайдер БД
        /// </summary>
        /// <param name="services">Коллекция сервисов приложения</param>
        /// <param name="configuration">Конфигурация приложения</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static IServiceCollection AddDatabaseProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDatabaseProvider(options =>
            {
                options = configuration.GetSection(ConfigKeys.Database.Get()).Get<DatabaseOptions>()
                    ?? throw new InvalidOperationException("Database configuration is missing");

                // Валидация значений
                if (!Enum.IsDefined(typeof(OrmTypes), options.OrmType))
                {
                    throw new ArgumentException($"Invalid ORM type: {options.OrmType}");
                }

                if (!Enum.IsDefined(typeof(ProviderTypes), options.Provider))
                {
                    throw new ArgumentException($"Invalid DB engine: {options.Provider}");
                }
            });
            return services;
        }
    }
}
