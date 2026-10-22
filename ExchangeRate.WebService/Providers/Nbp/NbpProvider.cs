using ExchangeRate.WebService.Exceptions;
using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers.Frankfurter.Responses;
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

    public async Task<IReadOnlyList<DailyRate>> GetRatesAsync(string fromCurrency, string toCurrency, DateOnly from, DateOnly to, CancellationToken cancellationToken)
    {
    var url = _options.Endpoints.Rates
    .Replace("{fromDate}", from.ToString("yyyy-MM-dd"))
    .Replace("{toDate}", to.ToString("yyyy-MM-dd"));

      var response = await _httpClient
    .GetAsync(url, cancellationToken);

      if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
      {
        throw new EmptyResponseException(
            "NBP returned an empty response.");
      }

      response.EnsureSuccessStatusCode();

      var responseJson = await response.Content.ReadFromJsonAsync<List<NbpTableResponse>>();

      var allNbpExchangeRatesDictionary = responseJson.SelectMany
      (
        table =>
        {
          return table.Rates
          .Select
          (rate => new DailyRate(
            Date: table.EffectiveDate,
            BaseCurrency: "PLN",
            TargetCurrency: rate.Code,
            rate.Mid)
          );
        }
      ).ToDictionary
      (
        rate => (rate.Date, rate.TargetCurrency),
        rate => rate
      );

      return ConvertRates(allNbpExchangeRatesDictionary, fromCurrency, toCurrency).ToList();
    }

    private static IEnumerable<DailyRate> ConvertRates(Dictionary<(DateOnly Date, string ToCurrency), DailyRate> plnExchangeRates, string fromCurrency, string toCurrency)
    {
      foreach (var date in plnExchangeRates.Keys.Select(x=>x.Date).ToList().Distinct())
      {
       
        if(fromCurrency == "PLN")
        {
          yield return new DailyRate(
            Date: date,
            BaseCurrency: fromCurrency,
            TargetCurrency: toCurrency,
            Value: Math.Round(1m / plnExchangeRates[(date, toCurrency)].Value, 4, MidpointRounding.AwayFromZero)
          );
          continue;
        }

        
        if (toCurrency == "PLN")
        {
          yield return new DailyRate(
            Date: date,
            BaseCurrency: fromCurrency,
            TargetCurrency: toCurrency,
            Value: plnExchangeRates[(date, fromCurrency)].Value
          );
          continue;
        }


        var toCurrencyRateInPlnForGivenDay = plnExchangeRates[(date, toCurrency)].Value;
        var fromCurrencyRateInPlnForGivenDay = plnExchangeRates[(date, fromCurrency)].Value;
        yield return new DailyRate(
          Date: date,
          BaseCurrency: fromCurrency,
          TargetCurrency: toCurrency,
          Value: Math.Round(fromCurrencyRateInPlnForGivenDay / toCurrencyRateInPlnForGivenDay, 4, MidpointRounding.AwayFromZero)
        );
      }
      
    }
  }
}
