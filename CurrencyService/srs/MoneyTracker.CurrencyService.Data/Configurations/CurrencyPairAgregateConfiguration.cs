using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using static MoneyTracker.CurrencyService.Data.CurrencyServiceContextConstants;


namespace MoneyTracker.CurrencyService.Data.Configurations
{
    /// <summary>
    /// Конфигурация таблицы для сущности <see cref="CurrencyPair"/>
    /// </summary>
    public class CurrencyPairAgregateConfiguration : IEntityTypeConfiguration<CurrencyPair>
    {
        public void Configure(EntityTypeBuilder<CurrencyPair> builder)
        {
            builder.ToTable(CurrencyPairTableName).HasKey(cp => cp.Id);
            builder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            builder.Ignore(cp => cp.DomainEvents);

            builder.Property(cp => cp.Id).IsRequired();
            builder.Property(cp => cp.IsActive).IsRequired().HasDefaultValue(true);

            builder.HasOne(cp => cp.BaseCurrency)
                .WithMany(BaseCurrencyPairsNavigationFieldName)
                .HasForeignKey(BaseCurrencyIdFkName)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cp => cp.TargetCurrency)
                .WithMany(TargetCurrencyPairsNavigationFieldName)
                .HasForeignKey(TargetCurrencyIdFkName)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(cp => cp.ExchangeRates)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
