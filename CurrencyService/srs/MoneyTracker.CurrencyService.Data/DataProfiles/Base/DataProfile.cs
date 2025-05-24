using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MoneyTracker.CurrencyService.Data.DataProfiles.Base
{
    public abstract class DataProfile<TDbContext> where TDbContext: DbContext
    {
        protected readonly DatabaseProfileConfig _config;

        protected DataProfile(DatabaseProfileConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Конфигурирует <typeparamref name="TDbContext"/>
        /// </summary>
        public abstract void ConfigureDbContext(DbContextOptionsBuilder builder);

        /// <summary>
        /// Инициализирует БД
        /// </summary>
        public virtual async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();

            if (_config.MigrationAssembly != null)
            {
                await context.Database.MigrateAsync();
            }

            if (ShouldSeedData())
            {
                await SeedDataAsync(context);
            }
        }

        protected virtual bool ShouldSeedData()
        => _config is not SqlServerConfig; // Пример логики

        protected abstract Task SeedDataAsync(TDbContext context);
    }
}
