using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTracker.Infrastructure.Data.Base;
using MoneyTracker.Infrastructure.Data.Configuration;
using MoneyTracker.Infrastructure.Data.Extensions;
using MoneyTracker.Infrastructure.Data.Providers;

namespace MoneyTracker.Infrastructure.UnitTests.Data
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddDatabaseProviderShouldRegisterEfCoreProvider()
        {
            IServiceCollection services = new ServiceCollection();
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    [ConfigKeys.Database.OrmType.Get()] = nameof(OrmTypes.EntityFrameworkCore),
                    [ConfigKeys.Database.Provider.Get()] = nameof(ProviderTypes.Postgres)
                })
                .Build();
            services.AddDatabaseProvider(config);
            IServiceProvider provider = services.BuildServiceProvider();

            provider.GetRequiredService<IDatabaseProvider>().Should().BeOfType<EfCoreDatabaseProvider>();
        }

        [Fact]
        public void AddDatabaseProviderShouldThrowOnUnsupportedProvider()
        {
            IServiceCollection services = new ServiceCollection();
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    [ConfigKeys.Database.OrmType.Get()] = "UnknownOrm",
                    [ConfigKeys.Database.Provider.Get()] = nameof(ProviderTypes.Postgres)
                })
                .Build();

            var action = () => services.AddDatabaseProvider(config);
            action.Should().Throw();
        }
    }
}
