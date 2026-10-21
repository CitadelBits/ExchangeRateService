using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.Providers.Nbp
{
  public class NbpProvider : IExchangeRateProvider
  {
    private readonly ProviderOptions _options;
    public string Code => Consts.ProviderCodes.Nbp;
    public string DisplayName => _options.DisplayName;
    public NbpProvider(IOptions<ExchangeRatesOptions> options)
    {
      _options = options.Value.Providers
          .FirstOrDefault(p => p.Code == Code)
          ?? throw new InvalidOperationException(
              $"Missing configuration for provider '{Code}'.");
    }
    public Task<IReadOnlyList<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }

    public Task<IReadOnlyList<DailyRate>> GetRatesAsync(string baseCurrency, string targetCurrency, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
