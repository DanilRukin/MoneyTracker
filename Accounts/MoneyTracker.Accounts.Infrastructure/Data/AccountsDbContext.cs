using Microsoft.EntityFrameworkCore;
using MoneyTracker.Accounts.Application.Outbox;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Currencies;
using MoneyTracker.Accounts.Domain.Transactions;
using MoneyTracker.Accounts.Domain.Transfers;
using System.Reflection;

namespace MoneyTracker.Accounts.Infrastructure.Data;

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

    public DbSet<Transfer> Transfers { get; set; }

    public DbSet<OutboxMessage> Outbox { get; set; }
}
