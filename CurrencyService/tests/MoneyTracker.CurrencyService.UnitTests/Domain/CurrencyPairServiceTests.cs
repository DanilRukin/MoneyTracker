using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using MoneyTracker.CurrencyService.Domain.Services;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public class CurrencyPairServiceTests : DomainBaseFixture
    {
        private CurrencyPairService _currencyPairService;
        public CurrencyPairServiceTests() 
        {
            _currencyPairService = new CurrencyPairService(CurrencyPairFactory);
        }
        [Fact]
        public void CanNotCreateCurrencyPairWhenCurrenciesAreSame()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = baseCurrency;
            var action = () => _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            action.Should().Throw<Exception>();
        }

        [Fact]
        public void CurrenciesAreKnowsAboutPairAfterItsCreation()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            baseCurrency.CurrencyPairs.Should().Contain(pair);
            targetCurrency.CurrencyPairs.Should().Contain(pair);
        }

        [Fact]
        public void CreatedCurrencyPairIsNotNull()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            pair.Should().NotBeNull();
        }

        [Fact]
        public void CreatedCurrencyPairHasPositiveId()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            pair.Id.Should().BePositive();
        }

        [Fact]
        public void SourceCurrenciesAreSameIntoThePair()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = CurrencyFactory.Create("2", "2", '2', true);
            CurrencyPair pair = _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            pair.BaseCurrency.Should().Be(baseCurrency);
            pair.TargetCurrency.Should().Be(targetCurrency);
        }
    }
}
