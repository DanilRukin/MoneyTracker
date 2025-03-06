using MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate;
using System.Reflection;

namespace MoneyTracker.CurrencyService.UnitTests.Domain.Fakes
{
    public class FakeCurrencyPairFactory : ICurrencyPairFactory
    {
        private int _currentId = 1;
        public CurrencyPair CreatePair(CurrencyPairBuilder builder)
        {

            CurrencyPair currencyPair = builder.Build();
            PropertyInfo idProperty = typeof(CurrencyPair).GetProperty(nameof(CurrencyPair.Id));
            if (idProperty is null)
            {
                throw new InvalidOperationException("У сущности валютной пары не существует свойства Id");
            }
            idProperty.SetValue(currencyPair, _currentId);
            _currentId++;
            return currencyPair;
        }
    }
}
