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

    public Task<IReadOnlyList<DailyRate>> GetRatesAsync(string baseCurrency, string targetCurrency, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}
