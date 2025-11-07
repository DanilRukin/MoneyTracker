using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MoneyTracker.Accounts.Domain.Currencies;
using MoneyTracker.Accounts.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Accounts.Tests.UnitTests.Infrastructure.Data
{
    public abstract class EfCoreBaseFixture : IDisposable
    {
        private readonly SqliteConnection _connection;
        internal readonly AccountsDbContext Context;

        protected EfCoreBaseFixture()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AccountsDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new AccountsDbContext(options);

            Context.Database.EnsureCreated();
        }

        internal async Task<Currency> GetDefaultCurrency()
        {
            Currency currency = await Context.Currencies.FirstAsync(c => c.Name == "dollar");
            return currency;
        }

        public void Dispose()
        {
            Context?.Dispose();
            _connection?.Dispose();
        }
    }
}
