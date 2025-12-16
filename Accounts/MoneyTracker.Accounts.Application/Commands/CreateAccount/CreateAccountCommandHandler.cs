using MediatR;
using MoneyTracker.Accounts.Application.Data;
using MoneyTracker.Accounts.Domain;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Currencies;
using SharedKernel.Interfaces;
using SharedKernel.Results;

namespace MoneyTracker.Accounts.Application.Commands.CreateAccount;

internal class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    internal CreateAccountCommandHandler(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Currency? currency = _unitOfWork.Currencies.GetByCode(request.CurrencyCode);
            if (currency is null)
                return Result<Guid>.NotFound("moneytracker.accounts.application.currency_not_found"); ;
            MoneyValue initialBalance = new MoneyValue(request.InitialBalance, currency);
            Account account = Account.Create(request.Name, initialBalance);

            _unitOfWork.Accounts.Save(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(account.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Error(ex.Message);
        }
    }
}
