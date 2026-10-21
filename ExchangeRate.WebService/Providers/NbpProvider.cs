using ExchangeRate.WebService.Models;

namespace ExchangeRate.WebService.Providers
{
  public class NbpProvider : IExchangeRateProvider
  {
    public string Code => throw new NotImplementedException();

    public string DisplayName => throw new NotImplementedException();

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
