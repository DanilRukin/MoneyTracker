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


            builder.HasIndex(ts => ts.Name)
                   .IsUnique();

            //builder.Ignore("Id");

            builder.HasData(
                new { Name = "Manual", Id = 1 }, // Ручное добавление
                new { Name = "BankImport", Id = 2 }, // Импорт из банка
                new { Name = "Recurring", Id = 3 }, // Регулярная операция
                new { Name = "System", Id = 4 } // Системная операция
            );
        }
    }
}
