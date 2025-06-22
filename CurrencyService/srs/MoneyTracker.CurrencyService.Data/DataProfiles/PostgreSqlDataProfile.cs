using Microsoft.EntityFrameworkCore;
using MoneyTracker.CurrencyService.Data.DataProfiles.Base;
using MoneyTracker.CurrencyService.Data.SeedData.Base;

namespace MoneyTracker.CurrencyService.Data.DataProfiles
{
    /// <summary>
    /// Профиль данных для PostgreSQL
    /// </summary>
    public class PostgreSqlDataProfile : DataProfile
    {
        private readonly IDataSeeder _dataSeeder;
        public PostgreSqlDataProfile(DatabaseProfileConfig config, IDataSeeder dataSeeder) : base(config)
        {
            _dataSeeder = dataSeeder;
        }

        public override void ConfigureDbContext(DbContextOptionsBuilder builder)
        {
            var options = _config.Options ?? new DatabaseProfileOptions();
            var connectionString = _config.ConnectionString;

            builder.UseNpgsql(connectionString, sql =>
            {
                if (_config.MigrationAssembly != null)
                {
                    sql.MigrationsAssembly(_config.MigrationAssembly);
                }

                if (options.EnableRetryOnFailure == true)
                {
                    sql.EnableRetryOnFailure();
                }

                if (options.CommandTimeout.HasValue)
                {
                    sql.CommandTimeout(options.CommandTimeout);
                }
            });
        }

        protected override Task SeedDataAsync()
        {
            return _dataSeeder.SeedData();
        }
    }
}
