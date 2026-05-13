using MoneyTracker.SharedKernel.DomainCore;
using MoneyTracker.SharedKernel.Results;

namespace MoneyTracker.Accounts.Contracts;

public interface IAccountsModule
{
    Task<Result<Guid>> CreateAccountAsync(string name, MoneyValue initialBalance, CancellationToken cancellationToken);

    Task<Result> UpdateBalanceAsync(Guid accountCode, MoneyValue balance, CancellationToken cancellationToken);

    Task<Result<MoneyValue>> GetBalanceAsync(Guid accountCode, CancellationToken cancellationToken);

    Task<Result> CloseAccountAsync(Guid accountCode, CancellationToken cancellationToken);
}
