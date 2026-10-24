namespace ExchangeRate.Blazor.WebServiceApi.Dtos
{
  public sealed record ProviderDto(string Code, string Name)
  {
      public override string ToString() => Name;
  }
}
