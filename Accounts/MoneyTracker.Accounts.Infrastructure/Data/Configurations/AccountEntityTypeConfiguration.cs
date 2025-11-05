using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Accounts;

namespace MoneyTracker.Accounts.Infrastructure.Data.Configurations
{
    internal class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(TableNames.Accounts, SchemaName.Name);

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .ValueGeneratedNever();

            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            // Owned Type для MoneyValue с ссылкой на Currency
            builder.OwnsOne(a => a.Balance, moneyBuilder =>
            {
                moneyBuilder.Property(m => m.Value)
                           .HasColumnName("Balance")
                           .HasPrecision(18, 2)
                           .IsRequired();

                // Связь с Currency вместо строки
                moneyBuilder.HasOne(m => m.Currency)
                           .WithMany()
                           .HasForeignKey("CurrencyId")
                           .OnDelete(DeleteBehavior.Restrict);
            });

            // Теневое свойство для CurrencyId
            builder.Property<int>("CurrencyId");

            builder.Property(a => a.IsActive)
                   .IsRequired();

            builder.Property(a => a.CreatedAt)
                   .IsRequired();

            builder.Property(a => a.UpdatedAt)
                   .IsRequired();

            builder.Property(a => a.ClosedAt)
                   .IsRequired(false);

            // Настройка связи с Transaction
            builder.HasMany(a => a.Transactions)
                   .WithOne()
                   .HasForeignKey("AccountId")
                   .OnDelete(DeleteBehavior.Cascade);

            // Индексы
            builder.HasIndex(a => a.IsActive);
            builder.HasIndex(a => a.CreatedAt);
            builder.HasIndex("CurrencyId");

            // Игнорируем доменные события в БД
            builder.Ignore(a => a.DomainEvents);
        }
    }
}
