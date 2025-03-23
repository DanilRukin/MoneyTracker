using System.Diagnostics.CodeAnalysis;

namespace MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages
{
    /// <summary>
    /// Текстовки ошибок класса <see cref="CurrencyPairAgregate.CurrencyPair"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CurrencyPairErrorMessages
    {
        public const string CanNotAddRateForAnotherPair = "Невозможно добавить курс валют, т.к. он не принадлежит этой паре";
        public const string CanNotSetBaseCurrenceSameAsTarget = "Нельзя установить базовую валюту такую же, как целевую!";
        public const string CanNotSetTargetCurrencySameAsBase = "Нельзя установить целевую валюту такую же, как базовую!";
        public const string CanNotCreatePairForArchivedCurrency = "Невозможно создать валютную пару для архивной валюты!";
        public const string CanNotActivatePairWhenBaseCurrencyIsArchived = "Невозможно активировать валютную пару, если базовая валюта архивна";
        public const string CanNotActivatePairWhenTargetCurrencyIsArchived = "Невозможно активировать валютную пару, если целевая валюта архивна";
    }
}
