using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MoneyTracker.CurrencyService.Data;
using Moq;
using SharedKernel.Interfaces;

namespace MoneyTracker.CurrencyService.UnitTests.Data
{
    public abstract class BaseEfTestFixture : IDisposable
    {
        protected SqliteConnection Connection { get; }
        protected DbContextOptions<CurrencyServiceContext> Options { get; }

        protected BaseEfTestFixture()
        {
            Connection = new SqliteConnection("Data Source=:memory:");
            Connection.Open();

            Options = new DbContextOptionsBuilder<CurrencyServiceContext>()
                .UseSqlite(Connection)
                .Options;

            Mock<IDomainEventDispatcher> dispatcherMock = new Mock<IDomainEventDispatcher>();

            using var context = new CurrencyServiceContext(dispatcherMock.Object, Options);
            context.Database.EnsureCreated();
        }
        protected CurrencyServiceContext GetClearContext()
        {
            Mock<IDomainEventDispatcher> dispatcherMock = new Mock<IDomainEventDispatcher>();
            return new CurrencyServiceContext(dispatcherMock.Object, Options);
        }

        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
