using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using static MoneyTracker.CurrencyService.Data.CurrencyServiceContextConstants;

namespace MoneyTracker.CurrencyService.Data.Configurations
{
    /// <summary>
    /// Конфигурация таблицы для сущности <see cref="Currency"/>
    /// </summary>
    public class CurrencyAgregateConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable(CurrencyTableName).HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Ignore(c => c.DomainEvents);
            builder.Ignore(c => c.OnThesePairsIsBase);
            builder.Ignore(c => c.OnThesePairsIsTarget);

            builder.Property(c => c.Id).IsRequired();
            builder.Property(c => c.Code).IsRequired().HasMaxLength(64);
            builder.Property(c => c.Symbol).IsRequired().HasMaxLength(1);
            builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(256);

            builder.HasMany<CurrencyPair>()
                .WithOne()
                .HasForeignKey(BaseCurrencyIdFkName)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasMany<CurrencyPair>()
                .WithOne()
                .HasForeignKey(TargetCurrencyIdFkName)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
