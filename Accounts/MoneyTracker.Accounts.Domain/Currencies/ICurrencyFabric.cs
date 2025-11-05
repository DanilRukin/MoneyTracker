namespace MoneyTracker.Accounts.Domain.Currencies;

internal interface ICurrencyFabric
{
    Currency CreateWithId(int id, string code, char symbol);
}