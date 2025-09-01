namespace MoneyTracker.CurrencyService.Data
{
    /// <summary>
    /// Константы, использующиеся в контексте <see cref="CurrencyServiceContext"/>
    /// </summary>
    internal static class CurrencyServiceContextConstants
    {
        internal const string CurrencyTableName = "Currency";
        internal const string CurrencyPairTableName = "CurremcyPairs";
        internal const string RateSourceTableName = "RateSource";
        internal const string ExchangeRatesTableName = "ExchnageRates";
        internal const string BaseCurrencyIdFkName = "BaseCurrencyId";
        internal const string TargetCurrencyIdFkName = "TargetCurrencyId";
        internal const string BaseCurrencyPairsNavigationFieldName = "_baseCurrencyPairs";
        internal const string TargetCurrencyPairsNavigationFieldName = "_targetCurrencyPairs";
    }
}
