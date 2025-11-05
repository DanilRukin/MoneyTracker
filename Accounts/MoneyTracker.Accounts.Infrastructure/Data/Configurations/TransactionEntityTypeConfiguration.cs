using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Transactions;

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

            // Owned Type для MoneyValue в Transaction с Currency
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

            // Теньные свойства для внешних ключей
            builder.Property<int>("CurrencyId"); // Для Amount.Currency
            builder.Property<int>("CategoryId");
            builder.Property<int>("TransactionSourceId");
            builder.Property<Guid>("AccountId");

            // Связь с Category
            builder.HasOne(t => t.Category)
                   .WithMany()
                   .HasForeignKey("CategoryId")
                   .OnDelete(DeleteBehavior.Restrict);

            // Связь с TransactionSource
            builder.HasOne(t => t.TransactionSource)
                   .WithMany()
                   .HasForeignKey("TransactionSourceId")
                   .OnDelete(DeleteBehavior.Restrict);

            // Индексы для производительности
            builder.HasIndex("AccountId");
            builder.HasIndex(t => t.CreatedAt);
            builder.HasIndex("CategoryId");
            builder.HasIndex("CurrencyId");
        }
    }
}
