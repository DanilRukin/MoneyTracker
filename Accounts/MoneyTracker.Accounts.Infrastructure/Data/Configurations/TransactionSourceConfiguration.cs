using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Transactions;

namespace MoneyTracker.Accounts.Infrastructure.Data.Configurations
{
    internal class TransactionSourceConfiguration : IEntityTypeConfiguration<TransactionSource>
    {
        public void Configure(EntityTypeBuilder<TransactionSource> builder)
        {
            builder.ToTable(TableNames.TransactionSources, SchemaName.Name);

            builder.Property<int>("Id")
                .ValueGeneratedOnAdd();

            builder.HasKey("Id");

            builder.Property(ts => ts.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.HasDiscriminator<string>("SourceType")
                .HasValue<TransactionSource>("Standard")
                .HasValue<TransferTransactionSource>("Transfer");


            builder.HasIndex(ts => ts.Name)
                   .IsUnique();

            builder.Ignore("Id");

            builder.HasData(
                new { Name = "Manual", SourceType = "Standard" }, // Ручное добавление
                new { Name = "BankImport", SourceType = "Standard" }, // Импорт из банка
                new { Name = "Recurring", SourceType = "Standard" }, // Регулярная операция
                new { Name = "System", SourceType = "Standard" }, // Системная операция
                new { Name = "TransferTransactionSource", SourceType = "Transfer" } // Перевод между счетами
            );
        }
    }
}
