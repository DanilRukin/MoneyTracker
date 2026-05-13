using MediatR;
using MoneyTracker.SharedKernel.DomainCore;
using MoneyTracker.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTracker.Accounts.Application.Queries.GetAccountBalance;

internal class GetAccountBalanceQuery : IRequest<Result<MoneyValue>>
{
    public Guid AccountCode { get; init; }
}
