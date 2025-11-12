using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Transactions;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Accounts;

namespace MoneyTracker.Accounts.Infrastructure.Data.Configurations
{
    internal class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable(TableNames.Transactions, SchemaName.Name);

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.AccountId)
                .IsRequired();

            // Owned Type для MoneyValue
            builder.OwnsOne(t => t.Amount, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Value)
                           .HasColumnName("Amount")
                           .HasPrecision(18, 2)
                           .IsRequired();

                // Связь с Currency
                moneyBuilder.HasOne(m => m.Currency)
                           .WithMany()
                           .HasForeignKey("CurrencyId")
                           .OnDelete(DeleteBehavior.Restrict);
            });

            // Теневое свойство для CurrencyId
            builder.Property<int>("CurrencyId")
                .IsRequired();

            // Связь с Category
            builder.HasOne(t => t.Category)
                   .WithMany()
                   .HasForeignKey("CategoryId")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property<int>("CategoryId")
                .IsRequired();

            // Связь с TransactionSource
            builder.HasOne(t => t.TransactionSource)
                   .WithMany()
                   .HasForeignKey("TransactionSourceId")
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property<int>("TransactionSourceId")
                .IsRequired();

            // TPH (Table Per Hierarchy) для наследования транзакций
            builder.HasDiscriminator<string>("TransactionType")
                .HasValue<IncomeTransaction>("Income")
                .HasValue<ExpenseTransaction>("Expense");

            // Индексы для производительности
            builder.HasIndex(t => t.AccountId);
            builder.HasIndex(t => t.CreatedAt);
            builder.HasIndex(t => new { t.AccountId, t.CreatedAt });
            builder.HasIndex("CategoryId");
            builder.HasIndex("TransactionSourceId");
            builder.HasIndex("CurrencyId");

            // Связь с Account (опционально, если нужна навигация)
            builder.HasOne<Account>()
                   .WithMany(a => a.Transactions)
                   .HasForeignKey(t => t.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(t => t.DomainEvents);
        }
    }
}