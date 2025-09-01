using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Data.Base;
using MoneyTracker.Infrastructure.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Infrastructure.UnitTests.Data
{
    public class DapperProviderTests
    {
        [Fact]
        public void ConfigureShouldRegisterIDbConnection()
        {
            var services = new ServiceCollection();
            var options = new DatabaseOptions
            {
                Provider = ProviderTypes.Sqlite,
                ConnectionString = "Data Source=:memory:",
                AutoMigrate = false
            };

            var provider = new DapperProvider();

            provider.Configure(services, options);
            IDbConnection connection = services.BuildServiceProvider().GetService<IDbConnection>();

            connection.Should().NotBeNull();
            connection.ConnectionString.Should().Be("Data Source=:memory:");
        }
    }
}
