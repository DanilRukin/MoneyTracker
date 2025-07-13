using Microsoft.EntityFrameworkCore;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.CurrencyService.Domain.Services;
using MoneyTracker.Infrastructure.Data.Base;

namespace MoneyTracker.CurrencyService.Data.SeedData
{
    /// <summary>
    /// Наполнитель данных с объектами, которые лежат в БД в виде отдельных объектов
    /// </summary>
    public class DataSeederWithInstancies : IDataSeeder
    {
        private readonly CurrencyServiceContext _context;
        private readonly CurrencyPairService _currencyPairService;
        public DataSeederWithInstancies(CurrencyServiceContext context, CurrencyPairService currencyPairService)
        {
            _context = context;
            _currencyPairService = currencyPairService;
        }
        private bool _empty = true;
        public Currency Dollar { get; private set; }
        public Currency Euro { get; private set; }
        public Currency Ruble { get; private set; }

        public CurrencyPair DollarRublePair { get; private set; }
        public CurrencyPair EuroRublePair { get; private set; }
        public CurrencyPair DollarEuroPair { get; private set; }

        public RateSource RussianCentralBank { get; private set; }
        public RateSource SberBank { get; private set; }
        public RateSource AlphaBank { get; private set; }

        public ExchangeRate DollarRublePairMayRate { get; private set; }
        public ExchangeRate DollarRublePairJuneRate { get; private set; }
        public ExchangeRate EuroRublePairMayRate { get; private set; }
        public ExchangeRate EuroRublePairJuneRate { get; private set; }
        public ExchangeRate DollarEuroPairPairMayRate { get; private set; }
        public ExchangeRate DollarEuroPairPairJuneRate { get; private set; }



        public async Task SeedDataAsync(CancellationToken token)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await ClearDatabaseAsync();
                if (!await _context.Currencies.AnyAsync())
                {
                    await FillCurrenciesAsync();
                    await FillRateSourcesAsync();
                    await FillCurrencyPirsAsync();
                    await FillExchangeRatesAsync();
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task FillCurrenciesAsync()
        {
            Dollar = _context.Create("dol", "Американский доллар", '$', true);
            Ruble = _context.Create("rub", "Российский рубль", '₽', true);
            Euro = _context.Create("eur", "Валюта Евросоюза", '€', true);
            await Task.Yield();
        }

        private async Task FillRateSourcesAsync()
        {
            RussianCentralBank = _context.CreateRateSource("Центральный банк РФ");
            SberBank = _context.CreateRateSource("Сбербанк");
            AlphaBank = _context.CreateRateSource("Альфа-Банк");
            await Task.Yield();
        }

        private async Task FillCurrencyPirsAsync()
        {
            DollarRublePair = _currencyPairService.CreatePair(Dollar, Ruble);
            EuroRublePair = _currencyPairService.CreatePair(Euro, Ruble);
            DollarEuroPair = _currencyPairService.CreatePair(Dollar, Euro);
            _context.SaveChanges();
            await Task.Yield();
        }

        private async Task FillExchangeRatesAsync()
        {
            int year = DateTime.Now.Year;
            int mayMonth = 5, juneMonth = 6, day = 1;
            DateTime may = new DateTime(year, mayMonth, day);
            DateTime june = new DateTime(year, juneMonth, day);

            DollarRublePairMayRate = _context.CreateRate(10.22m, may, DollarRublePair, RussianCentralBank);
            DollarRublePairJuneRate = _context.CreateRate(11.25m, june, DollarRublePair, RussianCentralBank);

            EuroRublePairMayRate = _context.CreateRate(10.22m, may, EuroRublePair, SberBank);
            EuroRublePairJuneRate = _context.CreateRate(11.25m, june, EuroRublePair, AlphaBank);

            DollarEuroPairPairMayRate = _context.CreateRate(10.22m, may, DollarEuroPair, RussianCentralBank);
            DollarEuroPairPairJuneRate = _context.CreateRate(11.25m, june, DollarEuroPair, SberBank);

            //_context.SaveChanges();
            await Task.Yield();
        }

        public async Task ClearDatabaseAsync()
        {
            if (_context.Currencies.Any())
                _context.Currencies.RemoveRange(_context.Currencies);

            if (_context.RateSources.Any())
                _context.RemoveRange(_context.RateSources);

            if (_context.CurrencyPairs.Any())
                _context.RemoveRange(_context.CurrencyPairs);

            if (_context.ExchangeRates.Any())
                _context.RemoveRange(_context.ExchangeRates);

            await _context.SaveChangesAsync();

            Dollar = Euro = Ruble = null;
            DollarRublePair = DollarEuroPair = EuroRublePair = null;
            RussianCentralBank = SberBank = AlphaBank = null;
            DollarEuroPairPairJuneRate = DollarEuroPairPairMayRate =
                DollarRublePairMayRate = DollarRublePairJuneRate =
                EuroRublePairMayRate = EuroRublePairJuneRate = null;
        }
    }
}
