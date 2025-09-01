using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using static MoneyTracker.CurrencyService.Data.CurrencyServiceContextConstants;


namespace MoneyTracker.CurrencyService.Data.Configurations
{
    /// <summary>
    /// Конфигурация таблицы для сущности <see cref="RateSource"/>
    /// </summary>
    public class RateSourceEntityConfiguration : IEntityTypeConfiguration<RateSource>
    {
        public void Configure(EntityTypeBuilder<RateSource> builder)
        {
            builder.ToTable(RateSourceTableName).HasKey(r => r.Id);
            builder.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();

            builder.Ignore(r => r.DomainEvents);

            builder.Property(r => r.Name).IsRequired().HasMaxLength(256);

            builder.HasMany(r => r.ExchangeRates)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}
