using MoneyTracker.CurrencyService.Domain.CurrencyAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.UnitTests.Domain.Fakes
{
    public class FakeCurrencyFactory : ICurrencyFactory
    {
        private int _currencyId = 1;
        public Currency Create(string code, string name, char sumbol, bool isActive)
        {
            Currency currency = new Currency(code, name, sumbol, isActive);
            PropertyInfo idProperty = typeof(Currency).GetProperty(nameof(Currency.Id));
            if (idProperty is null)
            {
                throw new InvalidOperationException("У сущности валюты не существует свойства Id");
            }
            idProperty.SetValue(currency, _currencyId);
            _currencyId++;
            return currency;
        }
    }
}
