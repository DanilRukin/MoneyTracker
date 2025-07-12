using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Data.Base;
using MoneyTracker.Infrastructure.Data.Providers;

namespace MoneyTracker.Infrastructure.UnitTests.Data
{
    public class EfCoreProviderTests
    {
        [Fact]
        public void ShouldRegisterDbContext()
        {
            IServiceCollection services = new ServiceCollection();
            DatabaseOptions options = new()
            {
                Provider = ProviderTypes.Postgres,
                ConnectionString = "Host=localhost;"
            };

            EfCoreDatabaseProvider provider = new();

            provider.Configure(services, options);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            serviceProvider.GetService<DbContext>().Should().NotBeNull();
        }
    }
}
