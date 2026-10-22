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
      var fromNormalized = (from.HasValue ? from.Value : DateOnly.FromDateTime(DateTime.Now));
      var toNormalized = (to.HasValue ? to.Value : DateOnly.FromDateTime(DateTime.Now));

      if (fromNormalized > toNormalized)
      {
        throw new ArgumentException(
            "Start date cannot be later than end date.");
      }

      var provider = _providerResolver.Resolve(providerCode);
      return (await provider.GetRatesAsync(fromCurrency.ToUpper(), toCurrency.ToUpper(), fromNormalized, toNormalized, cancellationToken))
        .OrderBy(x => x.Date).ToList();
     
    }
  }
}
