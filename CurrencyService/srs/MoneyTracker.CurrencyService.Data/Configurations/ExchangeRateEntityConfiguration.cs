using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using static MoneyTracker.CurrencyService.Data.CurrencyServiceContextConstants;


namespace MoneyTracker.CurrencyService.Data.Configurations
{
    /// <summary>
    /// Конфигурация таблицы для сущности <see cref="ExchangeRate"/>
    /// </summary>
    public class ExchangeRateEntityConfiguration : IEntityTypeConfiguration<ExchangeRate>
    {
        public void Configure(EntityTypeBuilder<ExchangeRate> builder)
        {
            builder.ToTable(ExchangeRatesTableName).HasKey(e => e.Id);
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Ignore(e => e.DomainEvents);

            builder.Property(e => e.Rate).IsRequired();
            builder.Property(e => e.RateDate).IsRequired();

            builder.HasOne(e => e.CurrencyPair)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.RateSource)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
