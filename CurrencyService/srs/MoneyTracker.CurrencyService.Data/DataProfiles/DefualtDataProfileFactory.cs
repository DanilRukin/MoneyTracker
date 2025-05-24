using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MoneyTracker.CurrencyService.Data.DataProfiles.Base;
using MoneyTracker.CurrencyService.Data.SeedData.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public DataProfile<TDbContext> CreateProfile<TDbContext>() where TDbContext: DbContext
        {
            var activeProfile = _config["Database:ActiveProfile"];
            var profileConfig = _config.GetSection($"Database:Profiles:{activeProfile}");

            return activeProfile switch
            {
                "Postgres" => new PostgreSqlDataProfile(
                    profileConfig.Get<PostgresConfig>() ?? throw new InvalidOperationException("Invalid Postgres config"), _dataSeeder),

                _ => throw new InvalidOperationException($"Unsupported database profile: {activeProfile}")
            };
        }
    }
}
