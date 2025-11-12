using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Transfers;

namespace MoneyTracker.Accounts.Infrastructure.Data.Configurations
{
    internal class TransferEntityTypeConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.ToTable(TableNames.Transfers, SchemaName.Name);

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id)
                .ValueGeneratedNever();

            builder.Property(t => t.FromAccountId)
                .IsRequired();
            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(t => t.FromAccountId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Property(t => t.ToAccountId)
                .IsRequired();
            builder.HasOne<Account>()
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(t => t.TransferDate)
                .IsRequired();

            builder.Property(t => t.CompletedAt)
                .IsRequired(false);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.Property(t => t.Description)
                .HasMaxLength(256)
                .IsRequired(false);

            builder.Property<int>("CurrencyId")
                .IsRequired();

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

            builder.Property(t => t.Status)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();


            builder.Property(t => t.ExpenseTransactionId)
                .IsRequired(false);

            builder.Property(t => t.IncomeTransactionId)
                .IsRequired(false);

            builder.HasIndex(t => new { t.FromAccountId, t.ToAccountId });


            builder.Ignore(t => t.DomainEvents);
        }
    }
}
