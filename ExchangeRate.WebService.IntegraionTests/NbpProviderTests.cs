using ExchangeRate.WebService.Exceptions;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers.Nbp;
using Microsoft.Extensions.DependencyInjection;
using ExtOptions = Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.IntegraionTests
{
  public class Tests
  {
    NbpProvider _sut;

    [SetUp]
    public void Setup()
    {
      var services = new ServiceCollection();

      services.AddHttpClient("nbp", client =>
      {
        client.BaseAddress = new Uri("https://api.nbp.pl/");
        client.Timeout = TimeSpan.FromSeconds(15);
      });

      ExchangeRatesOptions exchangeRateOptions = new ExchangeRatesOptions
      {
        DateFormat = "yyyy-MM-dd",
        DefaultProvider = "nbp",
        CurrencySort = new CurrencySortOptions
        {
          Field = SortField.Code,
          Direction = SortDirection.Descending
        },
        Providers = new List<ProviderOptions>
        {
          new ProviderOptions
          {
            Code = "nbp",
            DisplayName = "Narodowy Bank Polski",
            BaseUrl = "https://api.nbp.pl/",
            TimeoutSeconds = 15,
            Endpoints = new ProviderEndpoints
            {
              Currencies = "api/exchangerates/tables/A?format=json",
              Rates = "api/exchangerates/tables/A/{fromDate}/{toDate}?format=json"
            },
          }
        }
      };

      var serviceProvider = services.BuildServiceProvider();
      var options = ExtOptions.Options.Create(exchangeRateOptions);

      _sut = new NbpProvider(
          options,
          serviceProvider.GetRequiredService<IHttpClientFactory>());
    }

    [Test]
    public void GetRatesAsync_InvalidSourceCurrency_ShouldThrowUnrecognizedCurrencyException()
    {
      var fromCurrency = "QQQ";
      var toCurrency = "USD";
      var from = new DateOnly(2026, 1, 2);
      var to = new DateOnly(2026, 1, 3);
      using var cts = new CancellationTokenSource();

      Assert.ThrowsAsync<UnrecognizedCurrency>(async () =>
          await _sut.GetRatesAsync(
              fromCurrency,
              toCurrency,
              from,
              to,
              cts.Token));
    }

    [Test]
    public void GetRatesAsync_InvalidTargetCurrency_ShouldThrowUnrecognizedCurrencyException()
    {
      var fromCurrency = "USD";
      var toCurrency = "QQQ";
      var from = new DateOnly(2026, 1, 2);
      var to = new DateOnly(2026, 1, 3);
      using var cts = new CancellationTokenSource();

      Assert.ThrowsAsync<UnrecognizedCurrency>(async () =>
          await _sut.GetRatesAsync(
              fromCurrency,
              toCurrency,
              from,
              to,
              cts.Token));
    }


  }










}
