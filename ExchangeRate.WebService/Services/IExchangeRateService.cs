using ExchangeRate.WebService.Models;

namespace ExchangeRate.WebService.Services
{
  public interface IExchangeRateService
  {
    Task<IReadOnlyList<Currency>> GetCurrenciesAsync(
        string providerCode,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<DailyRate>> GetRatesAsync(
        string providerCode,
        string fromCurrency,
        string toCurrency,
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken);
  }
}
