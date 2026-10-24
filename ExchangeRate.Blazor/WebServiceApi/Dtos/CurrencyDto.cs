namespace ExchangeRate.Blazor.WebServiceApi.Dtos;

public sealed record CurrencyDto(string Code, string Name)
{
    public override string ToString() => $"{Code} — {Name}";
}
