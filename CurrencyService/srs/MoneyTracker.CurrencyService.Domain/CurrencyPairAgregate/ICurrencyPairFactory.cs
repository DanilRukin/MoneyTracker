using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Domain.CurrencyPairAgregate
{
    /// <summary>
    /// Фабрика валютных пар
    /// </summary>
    public interface ICurrencyPairFactory
    {
        /// <summary>
        /// Создает валютную пару из полученной заготовки
        /// </summary>
        /// <param name="builder">Заготовка валютной пары</param>
        /// <returns>Валютная пара с установленным id</returns>
        CurrencyPair CreatePair(CurrencyPairBuilder builder);
    }
}
