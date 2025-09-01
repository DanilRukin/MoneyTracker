using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.ErrorCodes;
using MoneyTracker.Infrastructure.Data.Base;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Driver;

namespace MoneyTracker.Infrastructure.Data.Providers
{
    /// <summary>
    /// Провайдер БД под NHibernate (В разработке)
    /// </summary>
    public class NHibernateProvider : IDatabaseProvider
    {
        /// <inheritdoc/>
        public IServiceCollection Configure(IServiceCollection services, DatabaseOptions options)
        {
            services.AddSingleton<ISessionFactory>(sp =>
            {
                var cfg = new NHibernate.Cfg.Configuration();

                // Настройка подключения
                cfg.DataBaseIntegration(db =>
                {
                    switch (options.Provider)
                    {
                        case ProviderTypes.Postgres:
                            db.Dialect<PostgreSQLDialect>();
                            db.Driver<NpgsqlDriver>();
                            db.ConnectionString = options.ConnectionString;
                            break;
                        case ProviderTypes.Sqlite:
                            db.Dialect<SQLiteDialect>();
                            db.Driver<SQLite20Driver>();
                            db.ConnectionString = options.ConnectionString;
                            break;
                        default:
                            throw new NotSupportedException(Errors.Infrastructure.Data.UnsupportedDataProvider);
                    }
                });

                // Добавляем маппинги (пример для Currency)
                //cfg.AddAssembly(typeof(Currency).Assembly);

                return cfg.BuildSessionFactory();
            });

            services.AddScoped<ISession>(sp =>
                sp.GetRequiredService<ISessionFactory>().OpenSession());

            return services;
        }

        /// <inheritdoc/>
        public async Task InitializeAsync(IServiceProvider services, DatabaseOptions options, CancellationToken token)
        {
            if (options.AutoMigrate)
            {
                // Для NHibernate используем FluentMigrator или ручные скрипты
                var session = services.GetRequiredService<ISession>();
                // Здесь можно выполнить SQL-скрипты миграции
                await session.CreateSQLQuery("CREATE TABLE IF NOT EXISTS...").ExecuteUpdateAsync();
            }
            await Task.CompletedTask;
        }
    }
}
