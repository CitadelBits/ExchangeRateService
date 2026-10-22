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
        string fromCurrency,
        string toCurrency,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken);
  }
}
