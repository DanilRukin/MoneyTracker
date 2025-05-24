using Microsoft.EntityFrameworkCore;
using MoneyTracker.CurrencyService.Data.SeedData.Base;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.ExchangeRateEntity;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;
using MoneyTracker.CurrencyService.Domain.Services;
using static MoneyTracker.CurrencyService.Data.CurrencyServiceContextConstants;

namespace MoneyTracker.CurrencyService.Data.SeedData
{
    /// <summary>
    /// Наполнитель данных рандомным способом
    /// </summary>
    public class RandomDataSeeder : IDataSeeder
    {
        private readonly CurrencyServiceContext _context;
        private readonly CurrencyPairService _pairService;

        public RandomDataSeeder(
            CurrencyServiceContext context,
            CurrencyPairService pairService)
        {
            _context = context;
            _pairService = pairService;
        }

        public async Task SeedData(bool clearExisting = false)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (clearExisting)
                {
                    await ClearDatabaseAsync();
                }

                if (!await _context.Currencies.AnyAsync())
                {
                    await SeedCurrenciesAsync();
                    await SeedRateSourcesAsync();
                    await SeedCurrencyPairsAsync();
                    await SeedExchangeRatesAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task SeedCurrenciesAsync()
        {
            var currencies = new[]
            {
                new Currency("USD", "US Dollar", '$', true),
                new Currency("EUR", "Euro", '€', true),
                new Currency("RUB", "Russian Ruble", '₽', true)
            };

            await _context.Currencies.AddRangeAsync(currencies);
            await _context.SaveChangesAsync();
        }

        private async Task SeedRateSourcesAsync()
        {
            var sources = new[]
            {
                new RateSource("Central Bank of Russia"),
                new RateSource("Sberbank"),
                new RateSource("Alpha Bank")
            };

            await _context.RateSources.AddRangeAsync(sources);
            await _context.SaveChangesAsync();
        }

        private async Task SeedCurrencyPairsAsync()
        {
            var usd = await _context.Currencies.FirstAsync(c => c.Code == "USD");
            var eur = await _context.Currencies.FirstAsync(c => c.Code == "EUR");
            var rub = await _context.Currencies.FirstAsync(c => c.Code == "RUB");

            _pairService.CreatePair(usd, rub);
            _pairService.CreatePair(eur, rub);
            _pairService.CreatePair(usd, eur);
        }

        private async Task SeedExchangeRatesAsync()
        {
            var currentYear = DateTime.Now.Year;
            var pairs = await _context.CurrencyPairs.ToListAsync();
            var sources = await _context.RateSources.ToListAsync();

            var rates = new List<ExchangeRate>();
            var rand = new Random();

            foreach (var pair in pairs)
            {
                foreach (var source in sources)
                {
                    for (var month = 1; month <= 6; month++)
                    {
                        rates.Add(_context.CreateRate(
                            (decimal)(rand.NextDouble() * 10 + 70), // 70-80
                            new DateTime(currentYear, month, 1),
                            pair,
                            source));
                    }
                }
            }

            await _context.ExchangeRates.AddRangeAsync(rates);
            await _context.SaveChangesAsync();
        }

        private async Task ClearDatabaseAsync()
        {
            await _context.Database.ExecuteSqlRawAsync(@$"
                DELETE FROM {CurrencyTableName};
                DELETE FROM {RateSourceTableName};
                DELETE FROM {CurrencyPairTableName};
                DELETE FROM {ExchangeRatesTableName};
            ");
        }
    }
}
