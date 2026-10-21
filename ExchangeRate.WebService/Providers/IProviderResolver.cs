namespace ExchangeRate.WebService.Providers
{
  public interface IProviderResolver
  {
    IExchangeRateProvider Resolve(string providerCode);
    IReadOnlyList<IExchangeRateProvider> GetAll();
  }
}
