using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.Accounts.Domain.Currencies;

namespace MoneyTracker.Accounts.Infrastructure.Data.Configurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable(TableNames.Currencies, SchemaName.Name);

            // Shadow property для Id
            builder.Property<int>("Id")
                   .ValueGeneratedOnAdd();

            builder.HasKey("Id");

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(64);

            builder.Property(c => c.Symbol)
                   .IsRequired()
                   .HasMaxLength(1)
                   .IsFixedLength()
                   .HasColumnType("char(1)");

            // Уникальные индексы
            builder.HasIndex(c => c.Name)
                   .IsUnique();

            builder.HasIndex(c => c.Symbol)
                   .IsUnique();

            // Игнорируем protected Id из доменной модели
            //builder.Ignore("Id");

            // Предопределенные валюты
            builder.HasData(
                new { Name = "US Dollar", Symbol = '$', Id = 1 },
                new { Name = "Euro", Symbol = '€', Id = 2 },
                new { Name = "Russian Ruble", Symbol = '₽', Id = 3 },
                new { Name = "British Pound", Symbol = '£', Id = 4 },
                new { Name = "Japanese Yen", Symbol = '¥', Id = 5 }
            );
        }
    }
}
