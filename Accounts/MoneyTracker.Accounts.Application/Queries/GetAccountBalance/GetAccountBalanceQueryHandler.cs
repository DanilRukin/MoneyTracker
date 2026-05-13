using MediatR;
using MoneyTracker.SharedKernel.DomainCore;
using MoneyTracker.SharedKernel.Results;

namespace MoneyTracker.Accounts.Application.Queries.GetAccountBalance;

internal class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, Result<MoneyValue>>
{
    public Task<Result<MoneyValue>> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
