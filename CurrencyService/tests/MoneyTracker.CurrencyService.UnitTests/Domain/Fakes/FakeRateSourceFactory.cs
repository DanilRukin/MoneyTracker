using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyTracker.CurrencyService.Domain.RateSourceEntity;

namespace MoneyTracker.CurrencyService.UnitTests.Domain.Fakes
{
    /// <summary>
    /// Фейковая реализация фабрики <see cref="RateSource"/>
    /// </summary>
    public class FakeRateSourceFactory : IRateSourceFactory
    {
        private int _currentId = 1;
        public RateSource CreateRateSource(string name)
        {
            RateSource source = new RateSource(name);
            var idProperty = typeof(RateSource).GetProperty(nameof(RateSource.Id));
            if (idProperty is null)
            {
                throw new InvalidOperationException("У сущности источника валют не существует свойства Id");
            }
            idProperty.SetValue(source, _currentId);
            _currentId++;
            return source;
        }
    }
}
