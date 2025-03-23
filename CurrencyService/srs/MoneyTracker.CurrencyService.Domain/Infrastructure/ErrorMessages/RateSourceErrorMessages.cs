using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages
{
    /// <summary>
    /// Текстовки ошибок класса <see cref="RateSourceEntity.RateSource"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RateSourceErrorMessages
    {
        public const string CanNotSetEmptySourceName = "Нельзя присвоить пустое имя источнику курса валют";
        public const string CanNotSetNameStartingWithSpaces = "Имя не должно начинаться с символа пробела";
        public const string CanNotSetNameEndingWithSpaces = "Имя не может заканчиваться символом пробела";
    }
}
