using Microsoft.EntityFrameworkCore;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using SharedKernel.Interfaces;
using System.Reflection;

namespace MoneyTracker.CurrencyService.Data
{
    /// <summary>
    /// Контекст данных CurrencyService
    /// </summary>
    public class CurrencyServiceContext : DbContext, ICurrencyPairFactory, IExchangeRateFactory, ICurrencyFactory, IRateSourceFactory
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public CurrencyServiceContext(IDomainEventDispatcher domainEventDispatcher, DbContextOptions<CurrencyServiceContext> options) : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher ?? throw new ArgumentNullException(nameof(domainEventDispatcher));
        }

        /// <summary>
        /// Валюты
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// Валютные пары
        /// </summary>
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }

        /// <summary>
        /// Источники курсов
        /// </summary>
        public DbSet<RateSource> RateSources { get; set; }

        /// <summary>
        /// Обменные курсы
        /// </summary>
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            base.ConfigureConventions(builder);
            builder.Properties<DateTime>()
                .HaveConversion<Converters.DateTimeUtcConverter>();
        }

        /// <summary>
        /// Сохраняет сущности
        /// </summary>
        public async Task SaveEntitiesAsync(CancellationToken cancellationToken)
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            var events = ChangeTracker.Entries<IDomainObject>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();
            await _domainEventDispatcher.DispatchAndClearEvents(events);
        }

        public ExchangeRate CreateRate(decimal rate, DateTime rateDate, CurrencyPair currencyPair, RateSource rateSource)
        {
            ExchangeRateBuilder builder = new ExchangeRateBuilder();
            builder
                .WithRate(rate)
                .FromSource(rateSource)
                .ForPair(currencyPair)
                .OnDate(rateDate);
            ExchangeRate exchangeRate = builder.Build();
            ExchangeRates.Add(exchangeRate);
            SaveChanges();
            return exchangeRate;
        }

        public CurrencyPair CreatePair(CurrencyPairBuilder builder)
        {
            CurrencyPair currencyPair = builder.Build();
            CurrencyPairs.Add(currencyPair);
            SaveChanges();
            return currencyPair;
        }

        public RateSource CreateRateSource(string name)
        {
            RateSource source = new(name);
            RateSources.Add(source);
            SaveChanges();
            return source;
        }

        public Currency Create(string code, string name, char sumbol, bool isActive)
        {
            Currency currency = new(code, name, sumbol, isActive);
            Currencies.Add(currency);
            SaveChanges();
            return currency;
        }
    }
}
