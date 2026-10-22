using ExchangeRate.WebService.Exceptions;

namespace ExchangeRate.WebService.Providers
{
  public class ProviderResolver : IProviderResolver
  {

    private readonly IReadOnlyDictionary<string, IExchangeRateProvider>
        _providers;

    public ProviderResolver(
        IEnumerable<IExchangeRateProvider> providers)
    {
      _providers = providers.ToDictionary(
          p => p.Code,
          StringComparer.OrdinalIgnoreCase);
    }

    public IExchangeRateProvider Resolve(string providerCode)
    {
      if (_providers.TryGetValue(providerCode, out var provider))
        return provider;

      throw new UnknownProviderException(providerCode);
    }

    public IReadOnlyList<IExchangeRateProvider> GetAll()
        => _providers.Values.ToList();

  }
}
