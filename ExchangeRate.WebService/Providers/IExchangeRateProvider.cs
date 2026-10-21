using ExchangeRate.WebService.Models;

namespace ExchangeRate.WebService.Providers
{
  public interface IExchangeRateProvider
  {
    string Code { get; }

    string DisplayName { get; }

    Task<IReadOnlyList<Currency>> GetCurrenciesAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<DailyRate>> GetRatesAsync(
        string baseCurrency,
        string targetCurrency,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken);
  }
}
