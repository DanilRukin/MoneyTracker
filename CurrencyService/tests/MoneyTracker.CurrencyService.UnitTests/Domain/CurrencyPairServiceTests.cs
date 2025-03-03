using FluentAssertions;
using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using MoneyTracker.CurrencyService.Domain.Services;

namespace MoneyTracker.CurrencyService.UnitTests.Domain
{
    public class CurrencyPairServiceTests : DomainBaseFixture
    {
        private CurrencyPairService _currencyPairService;
        public CurrencyPairServiceTests() 
        {
            _currencyPairService = new CurrencyPairService();
        }
        [Fact]
        public void CanNotCreateCurrencyPairWhenCurrenciesAreSame()
        {
            Currency baseCurrency = CurrencyFactory.Create("1", "1", '1', true);
            Currency targetCurrency = baseCurrency;
            var action = () => _currencyPairService.CreatePair(baseCurrency, targetCurrency);
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
