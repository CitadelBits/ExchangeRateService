using ExchangeRate.WebService.Dtos;
using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.Services
{
  public class ExchangeRateService : IExchangeRateService
  {
    private readonly IProviderResolver _providerResolver;
    IOptions<ExchangeRatesOptions> _exchangeRateOptions;

    public ExchangeRateService(IProviderResolver providerResolver, IOptions<ExchangeRatesOptions> exchangeRateOptions)
    {
      _providerResolver = providerResolver;
      _exchangeRateOptions = exchangeRateOptions;
    }

    public async Task<IReadOnlyList<Currency>> GetCurrenciesAsync(string providerCode, CancellationToken cancellationToken)
    {
      var provider = _providerResolver.Resolve(providerCode);

      var currencies = await provider.GetCurrenciesAsync(cancellationToken);
      var currenciesSorted = _exchangeRateOptions.Value.CurrencySort.Field switch
      {
        SortField.Name => _exchangeRateOptions.Value.CurrencySort.Direction == SortDirection.Descending
            ? currencies.OrderByDescending(x => x.Name)
            : currencies.OrderBy(x => x.Name),

        SortField.Code => _exchangeRateOptions.Value.CurrencySort.Direction == SortDirection.Descending
            ? currencies.OrderByDescending(x => x.Code)
            : currencies.OrderBy(x => x.Code),

        _ => currencies.OrderBy(x => x.Code)
      };

      return currenciesSorted.ToList();
    }

    public async Task<IReadOnlyList<DailyRate>> GetRatesAsync(string providerCode, string fromCurrency, string toCurrency, DateOnly? from, DateOnly? to, CancellationToken cancellationToken)
    {
      var today = DateOnly.FromDateTime(DateTime.Now);

      var fromNormalized = from.HasValue
          ? from.Value
          : today;

      var toNormalized = to.HasValue
          ? to.Value
          : today;

      validateParameters(providerCode, fromCurrency, toCurrency, fromNormalized, toNormalized);

      var provider = _providerResolver.Resolve(providerCode);
      return (await provider.GetRatesAsync(fromCurrency.ToUpper(), toCurrency.ToUpper(), fromNormalized, toNormalized, cancellationToken))
        .OrderBy(x => x.Date).ToList();

    }

    public async Task<ExchangeRateStatistics> GetStatisticsAsync(string providerCode, string fromCurrency, string toCurrency, DateOnly? from, DateOnly? to,
      CancellationToken cancellationToken)
    {
      var rates = await GetRatesAsync(
          providerCode,
          fromCurrency,
          toCurrency,
          from,
          to,
          cancellationToken);

      if (rates.Count == 0)
      {
        throw new InvalidOperationException(
            "No exchange rates available for the selected period.");
      }

      return new ExchangeRateStatistics(
          Math.Round(rates.Min(r => r.Value), 4, MidpointRounding.AwayFromZero),
          Math.Round(rates.Max(r => r.Value), 4, MidpointRounding.AwayFromZero),
          Math.Round(rates.Average(r => r.Value), 4, MidpointRounding.AwayFromZero),
          rates.Count);
    }

    private void validateParameters(string providerCode, string fromCurrency, string toCurrency, DateOnly? from, DateOnly? to)
    {
      if (from > to)
      {
        throw new ArgumentException(
            "Start date cannot be later than end date.");
      }

      if (string.IsNullOrWhiteSpace(fromCurrency) || string.IsNullOrWhiteSpace(toCurrency))
      {
        throw new ArgumentException(
            "From and To currencies must be specified.");
      }

      if (fromCurrency.Length != 3 || toCurrency.Length != 3)
      {
        throw new ArgumentException(
            "The currency code must be exactly 3 characters long.");
      }

      if (fromCurrency.Equals(toCurrency, StringComparison.OrdinalIgnoreCase))
      {
        throw new ArgumentException(
            "From and To currencies cannot be the same.");
      }

      if (string.IsNullOrWhiteSpace(providerCode))
      {
        throw new ArgumentException(
            "Provider code must be specified.");
      }
    }
  }
}
