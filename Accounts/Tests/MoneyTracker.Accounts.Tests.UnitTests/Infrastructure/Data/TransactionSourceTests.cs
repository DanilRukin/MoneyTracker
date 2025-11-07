using FluentAssertions;

namespace MoneyTracker.Accounts.Tests.UnitTests.Infrastructure.Data
{
    public class TransactionSourceTests : EfCoreBaseFixture
    {
        public TransactionSourceTests()
        {
        }

        [Fact]
        public void ShouldGetPreinstalledSources()
        {
            var sources = Context.TransactionSources.ToList();

            sources.Should().NotBeNullOrEmpty();
        }
    }
}
