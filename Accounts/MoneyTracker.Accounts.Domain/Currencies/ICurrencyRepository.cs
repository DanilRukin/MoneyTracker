namespace MoneyTracker.Accounts.Domain.Currencies;

internal interface ICurrencyRepository
{
    Currency GetByCode(string code);
}