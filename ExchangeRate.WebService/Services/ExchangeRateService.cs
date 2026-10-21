using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Providers;

namespace ExchangeRate.WebService.Services
{
  public class ExchangeRateService : IExchangeRateService
  {
    private readonly IProviderResolver _providerResolver;

    public ExchangeRateService(IProviderResolver providerResolver) 
    { 
      _providerResolver = providerResolver;
      
    }
    

    public async Task<IReadOnlyList<Currency>> GetCurrenciesAsync(string providerCode, CancellationToken cancellationToken)
    {
      var provider = _providerResolver.Resolve(providerCode);
      return await provider.GetCurrenciesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DailyRate>> GetRatesAsync(string providerCode, string fromCurrency, string toCurrency, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
