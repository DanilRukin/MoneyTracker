using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Data.Base;
using Npgsql;
using System.Data;

namespace MoneyTracker.Infrastructure.Data.Providers
{
    /// <summary>
    /// Провайдер БД под Dapper (В разработке)
    /// </summary>
    public class DapperProvider : IDatabaseProvider
    {
        public IServiceCollection Configure(IServiceCollection services, DatabaseOptions options)
        {
            services.AddScoped<IDbConnection>(_ =>
            {
                var connection = CreateConnection(options);
                connection.Open();
                return connection;
            });

            return services;
        }

        public Task InitializeAsync(IServiceProvider services, DatabaseOptions options, CancellationToken token)
        {
            // Dapper не требует миграций, но можно добавить скрипты
            if (options.AutoMigrate)
            {
                var connection = services.GetRequiredService<IDbConnection>();
                connection.Execute("CREATE TABLE IF NOT EXISTS...");
            }
            return Task.CompletedTask;
        }

        private IDbConnection CreateConnection(DatabaseOptions options)
        {
            return options.Provider switch
            {
                ProviderTypes.Sqlite => new SqliteConnection(options.ConnectionString),
                ProviderTypes.Postgres => new NpgsqlConnection(options.ConnectionString),
                _ => throw new NotSupportedException($"Unsupported provider: {options.Provider}")
            };
        }
    }
}
