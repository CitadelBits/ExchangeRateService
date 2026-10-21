using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers.Frankfurter.Responses;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.Providers.Frankfurter
{
  public class FrankfurterProvider : IExchangeRateProvider
  {
    private readonly ProviderOptions _options;

    public string Code => Consts.ProviderCodes.Frankfurter;
    public string DisplayName => _options.DisplayName;
    private readonly HttpClient _httpClient;
    public FrankfurterProvider(IOptions<ExchangeRatesOptions> options, IHttpClientFactory httpClientFactory)
    {
      _options = options.Value.Providers
          .FirstOrDefault(p => p.Code == Code)
          ?? throw new InvalidOperationException(
              $"Missing configuration for provider '{Code}'.");

      _httpClient = httpClientFactory.CreateClient(Code);
    }

    public async Task<IReadOnlyList<Currency>> GetCurrenciesAsync(CancellationToken cancellationToken)
    {
      var response = await _httpClient.GetFromJsonAsync<
          FrankfurterCurrencyResponse[]>(
          _options.Endpoints.Currencies,
          cancellationToken);

      if (response is null)
      {
        throw new InvalidOperationException(
            "Frankfurter API returned an empty response.");
      }

      return response
          .Select(currency => new Currency(
              currency.IsoCode,
              currency.Name))
          .ToList();
    }

    public async Task<IReadOnlyList<DailyRate>> GetRatesAsync(
        string fromCurrency,
        string toCurrency,
        DateOnly from,
        DateOnly to,
        CancellationToken ct)
    {
      var url =
          $"v2/rates?base={fromCurrency}" +
          $"&quotes={toCurrency}" +
          $"&from={from:yyyy-MM-dd}" +
          $"&to={to:yyyy-MM-dd}";

      var response = await _httpClient
          .GetFromJsonAsync<List<FrankfurterRateResponse>>(url, ct);

      if (response is null)
      {
        throw new InvalidOperationException(
            "Frankfurter returned an empty response.");
      }

      return response
          .Select(r => new DailyRate(
              r.Date,
              r.BaseCurrency,
              r.TargetCurrency,
              r.Rate))
          .OrderBy(r => r.Date)
          .ToList();
    }
  }
}
