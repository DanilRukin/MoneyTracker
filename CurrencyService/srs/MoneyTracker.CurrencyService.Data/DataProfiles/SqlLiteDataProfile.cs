using Microsoft.EntityFrameworkCore;
using MoneyTracker.CurrencyService.Data.DataProfiles.Base;
using MoneyTracker.CurrencyService.Data.SeedData.Base;

namespace MoneyTracker.CurrencyService.Data.DataProfiles
{
    public class SqlLiteDataProfile : DataProfile
    {
        private readonly IDataSeeder _dataSeeder;

        public SqlLiteDataProfile(DatabaseProfileConfig config, IDataSeeder dataSeeder) : base(config)
        {
            _dataSeeder = dataSeeder ?? throw new ArgumentNullException(nameof(dataSeeder));
        }

        public override void ConfigureDbContext(DbContextOptionsBuilder builder)
        {
            var options = _config.Options ?? new DatabaseProfileOptions();
            var connectionString = _config.ConnectionString;

            builder.UseSqlite(connectionString, sql =>
            {
                if (_config.MigrationAssembly != null)
                {
                    sql.MigrationsAssembly(_config.MigrationAssembly);
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
