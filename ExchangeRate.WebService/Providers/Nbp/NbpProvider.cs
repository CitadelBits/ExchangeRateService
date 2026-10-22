using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers.Nbp.Responses;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace ExchangeRate.WebService.Providers.Nbp
{
  public class NbpProvider : IExchangeRateProvider
  {
    private readonly ProviderOptions _options;
    private readonly HttpClient _httpClient;

    public string Code => Consts.ProviderCodes.Nbp;
    public string DisplayName => _options.DisplayName;
    public NbpProvider(IOptions<ExchangeRatesOptions> options, IHttpClientFactory httpClientFactory)
    {
      _options = options.Value.Providers
          .FirstOrDefault(p => p.Code == Code)
          ?? throw new InvalidOperationException(
              $"Missing configuration for provider '{Code}'.");

      _httpClient = httpClientFactory.CreateClient(Code);
    }
    public async Task<IReadOnlyList<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
      var tables = await _httpClient.GetFromJsonAsync<List<NbpTableResponse>>(
          _options.Endpoints.Currencies,
          cancellationToken);

      var currencies = tables
          .SelectMany(table => table.Rates)
          .Select(rate => new Currency(rate.Code, rate.Currency))
          .DistinctBy(currency => currency.Code)
          .ToList();

      currencies.Add(new Currency("PLN", "polski złoty"));

      return currencies
          .ToList();
    }

    public Task<IReadOnlyList<DailyRate>> GetRatesAsync(string baseCurrency, string targetCurrency, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
      if (from > to)
      {
        throw new ArgumentException(
            "Start date cannot be later than end date.");
      }

      return Task.FromResult<IReadOnlyList<DailyRate>>(Array.Empty<DailyRate>());
    }
  }
}
