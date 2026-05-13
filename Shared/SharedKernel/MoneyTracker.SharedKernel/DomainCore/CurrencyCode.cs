namespace MoneyTracker.SharedKernel.DomainCore;

public class CurrencyCode : ValueObject
{
    public string Code { get; protected set; } = default!;
    protected CurrencyCode() { }
    public CurrencyCode(string code)
    {
        SetCode(code);
    }

    private void SetCode(string code)
    {
        if (!IsValid(code))
            throw new ArgumentException($"Неверный код валюты: {code}");
        Code = code.ToUpperInvariant();
    }

    private bool IsValid(string code) =>
        code?.Length == 3 && code.All(char.IsLetter);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }

    public static implicit operator CurrencyCode(string code) => new CurrencyCode(code);
    public static explicit operator string(CurrencyCode code) => code.Code;
}
