using Microsoft.Extensions.Configuration;
using MoneyTracker.CurrencyService.Data.DataProfiles.Base;
using MoneyTracker.CurrencyService.Data.SeedData.Base;
using MoneyTracker.SharedConstants.ErrorCodes;
using static MoneyTracker.CurrencyService.Data.Infrastructure.ConfigKeys;
using static MoneyTracker.CurrencyService.Data.Infrastructure.DataProfileNames;

namespace MoneyTracker.CurrencyService.Data.DataProfiles
{
    public class DefaultDataProfileFactory : IDataProfileFactory
    {
        private readonly IConfiguration _config;
        private readonly IDataSeeder _dataSeeder;

        public DefaultDataProfileFactory(IConfiguration config, IDataSeeder dataSeeder)
        {
            _dataSeeder = dataSeeder;
            _config = config;
        }

        public DataProfile CreateProfile()
        {
            var activeProfile = _config[Database.ActiveProfile.Get()];
            var profileConfig = _config.GetSection(Database.Profiles.Get(activeProfile));

            return activeProfile switch
            {
                Postgres => new PostgreSqlDataProfile(
                    profileConfig.Get<PostgresConfig>() ?? throw new InvalidOperationException(Errors.Data.InvalidConfig), _dataSeeder),
                SqlServer => new SqlServerDataProfile(
                    profileConfig.Get<SqlServerConfig>() ?? throw new InvalidOperationException(Errors.Data.InvalidConfig), _dataSeeder),
                SQLite => new SqlLiteDataProfile(
                    profileConfig.Get<SQLiteConfig>() ?? throw new InvalidOperationException(Errors.Data.InvalidConfig), _dataSeeder),
                InMemory => new InMemoryDataProfile(
                    profileConfig.Get<InMemoryConfig>() ?? throw new InvalidOperationException(Errors.Data.InvalidConfig), _dataSeeder),
                _ => throw new InvalidOperationException(Errors.Data.UnsupportedProfile)
            };
        }
    }
}
