using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Currencies;
using MoneyTracker.Accounts.Domain.Transfers;

namespace MoneyTracker.Accounts.Application.Data;

/// <summary>
/// Unit of Work контекста Accounts
/// </summary>
internal interface IUnitOfWork // Добавляем функционал по мере необходимости, не городим все и сразу
{
    /// <summary>
    /// Счета
    /// </summary>
    IAccountsRepository Accounts { get; }

    /// <summary>
    /// Переводы
    /// </summary>
    ITransferRepository Transfers { get; }

    /// <summary>
    /// Валюты
    /// </summary>
    ICurrencyRepository Currencies { get; }

    /// <summary>
    /// Категории трат
    /// </summary>
    IExpenseCategoryRepository ExpenseCategories { get; }

    /// <summary>
    /// Категории доходов
    /// </summary>
    IIncomeCategoryRepository IncomeCategories { get; }

    /// <summary>
    /// Сохряняет изменения в базу
    /// </summary>
    /// <param name="cancellationToken">Токен завершения задачи</param>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
