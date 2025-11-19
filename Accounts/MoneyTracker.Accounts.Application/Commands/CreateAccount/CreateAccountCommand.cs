using MediatR;
using SharedKernel.Results;

namespace MoneyTracker.Accounts.Application.Commands.CreateAccount;

/// <summary>
/// Команда создания счета
/// </summary>
public class CreateAccountCommand : IRequest<Result<Guid>>
{
    /// <summary>
    /// Название счета
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Начальный баланс
    /// </summary>
    public required decimal InitialBalance { get; init; }

    /// <summary>
    /// Id валюты, в которой будет создан счет
    /// </summary>
    public required string CurrencyCode { get; init; }
}
