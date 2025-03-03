using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Domain.CurrencyAggregate
{
    /// <summary>
    /// Репозиторий валют
    /// </summary>
    public interface ICurrencyFactory
    {
        /// <summary>
        /// Создает валюту
        /// </summary>
        /// <param name="code">код валюты</param>
        /// <param name="name">полное наименование валюты</param>
        /// <param name="sumbol">символ валюты</param>
        /// <param name="isActive">признак активности валюты</param>
        /// <returns></returns>
        Currency Create(string code, string name, char sumbol, bool isActive);
    }
}
