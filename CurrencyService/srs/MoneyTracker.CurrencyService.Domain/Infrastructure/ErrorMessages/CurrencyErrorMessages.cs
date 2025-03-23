using System.Diagnostics.CodeAnalysis;

namespace MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages
{
    /// <summary>
    /// Текстовки ошибок класса <see cref="CurrencyAggregate.Currency"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CurrencyErrorMessages
    {
        public const string CanNotAddNullableCurrencyPair = "Нельзя добавить несуществующую валютную пару";
        public const string ThisCurrencyPairDoesNotBelongToCurrency = "Данная валютная пара не принадлежит этой валюте";
    }
}
