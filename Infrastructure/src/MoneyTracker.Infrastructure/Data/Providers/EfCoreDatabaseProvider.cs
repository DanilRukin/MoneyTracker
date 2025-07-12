using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Data.Base;

namespace MoneyTracker.Infrastructure.Data.Providers
{
    /// <summary>
    /// Провайдер конфигурации БД для EntityFrameworkCore
    /// </summary>
    public class EfCoreDatabaseProvider : IDatabaseProvider
    {
        public IServiceCollection Configure(IServiceCollection services, DatabaseOptions options)
        {
            services.AddDbContext<DbContext>(builder =>
            {
                switch (options.Provider)
                {
                    case ProviderTypes.Postgres:
                        builder.UseNpgsql(options.ConnectionString);
                        break;
                    case ProviderTypes.Sqlite:
                        builder.UseSqlite(options.ConnectionString);
                        break;
                    case ProviderTypes.InMemory:
                        builder.UseInMemoryDatabase(options.ConnectionString);
                        break;
                    default:
                        throw new NotSupportedException($"Unsupported provider: {options.Provider}");
                }
            });

            return services;
        }

        public async Task InitializeAsync(IServiceProvider services, DatabaseOptions options, CancellationToken token)
        {
            if (options.AutoMigrate && options.Provider != ProviderTypes.InMemory)
            {
                using var scope = services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
                await dbContext.Database.MigrateAsync(token);
            }
        }
    }
}
