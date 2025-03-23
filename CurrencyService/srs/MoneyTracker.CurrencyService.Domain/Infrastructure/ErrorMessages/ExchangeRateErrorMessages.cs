using System.Diagnostics.CodeAnalysis;

namespace MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages
{
    /// <summary>
    /// Текстовки ошибок класса <see cref="ExchangeRateEntity.ExchangeRate"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ExchangeRateErrorMessages
    {
        public const string CanNotUpdateRateForNullableCUrrencyPair = "Нельзя обновить курс для неустановленной валютной пары";
        public const string RateCanNotBeNegative = "Курс не может быть меньше нуля.";
        public const string RateCouldBeUpdatedOnlyForActiveCurrencyPair = "Курс валют можно обновить только для активной валютной пары";
        public const string CanNotSetRateForArchivedPair = "Невозможно установить курс для архивной пары";
    }
}
