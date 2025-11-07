using Microsoft.EntityFrameworkCore;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Currencies;
using MoneyTracker.Accounts.Domain.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Accounts.Infrastructure.Data
{
    internal class AccountsDbContext : DbContext
    {
        public AccountsDbContext(DbContextOptions options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<TransactionSource> TransactionSources { get; set; }
    }
}
