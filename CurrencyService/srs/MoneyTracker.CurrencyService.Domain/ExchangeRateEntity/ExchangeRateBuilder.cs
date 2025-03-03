using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;

namespace MoneyTracker.CurrencyService.Domain.ExchangeRateEntity
{
    /// <summary>
    /// Билдер для <see cref="ExchangeRate"/>. Выполняет построение бизнес-сущности <see cref="ExchangeRate"/>,
    /// проверяя все возможные инварианты. Построение производится без привязки к id.
    /// </summary>
    public class ExchangeRateBuilder
    {
        public decimal Rate { get; private set; }
        public DateTime Date { get; private set; }
        public RateSource RateSource { get; private set; }

        public CurrencyPair Pair { get; private set; }

        public ExchangeRateBuilder WithRate(decimal rate)
        {
            Rate = rate;
            return this;
        }

        public ExchangeRateBuilder OnDate(DateTime onDate)
        {
            Date = onDate;
            return this;
        }

        public ExchangeRateBuilder ForPair(CurrencyPair pair)
        {
            Pair = pair;
            return this;
        }

        public ExchangeRateBuilder FromSource(RateSource source)
        {
            RateSource = source;
            return this;
        }

        public ExchangeRate Build() => new ExchangeRate(this);
    }
}
