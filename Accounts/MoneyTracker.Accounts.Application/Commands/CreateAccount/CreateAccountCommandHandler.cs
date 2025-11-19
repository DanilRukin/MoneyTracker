using MediatR;
using MoneyTracker.Accounts.Domain;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Currencies;
using SharedKernel.Interfaces;
using SharedKernel.Results;

namespace MoneyTracker.Accounts.Application.Commands.CreateAccount;

internal class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<Guid>>
{
    private readonly IAccountsRepository _accountsRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IDomainEventDispatcher _dispatcher;

    internal CreateAccountCommandHandler(
        IAccountsRepository accountsRepository,
        ICurrencyRepository currencyRepository,
        IDomainEventDispatcher dispatcher)
    {
        _accountsRepository = accountsRepository;
        _currencyRepository = currencyRepository;
        _dispatcher = dispatcher;
    }

    public async Task<Result<Guid>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Currency currency = _currencyRepository.GetByCode(request.CurrencyCode);
            if (currency is null)
                return Result<Guid>.NotFound("moneytracker.accounts.application.currency_not_found"); ;
            MoneyValue initialBalance = new MoneyValue(request.InitialBalance, currency);
            Account account = Account.Create(request.Name, initialBalance);

            _accountsRepository.Save(account);
            await _dispatcher.DispatchAndClearEvents([account]);

            return Result.Success(account.Id);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Error(ex.Message);
        }
    }
}
