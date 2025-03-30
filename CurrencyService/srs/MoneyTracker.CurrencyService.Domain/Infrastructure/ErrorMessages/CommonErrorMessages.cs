using System.Diagnostics.CodeAnalysis;

namespace MoneyTracker.CurrencyService.Domain.Infrastructure.ErrorMessages
{
    /// <summary>
    /// Общие сообщения об ошибках
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CommonErrorMessages
    {
        public const string CouldNotApplyOperationForDroppedEntity = "Невозможно выполнить операцию над удаленной сущностью";
    }
}
