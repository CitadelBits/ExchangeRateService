using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace ExchangeRate.WebService.IntegraionTests;

public class EndpointTests
{

  private WebApplicationFactory<Program> _factory = null!;
  private HttpClient _client = null!;

  [SetUp]
  public void Setup()
  {
    _factory = new WebApplicationFactory<Program>();
    _client = _factory.CreateClient();
  }

  [TearDown]
  public void TearDown()
  {
    _client.Dispose();
    _factory.Dispose();
  }


  [Test]
  [TestCase("nbp", "EUR", "PLN", "2026-01-05", "2026-01-09", HttpStatusCode.OK)]
  [TestCase("nbp", "PLN", "EUR", "2026-01-05", "2026-01-09", HttpStatusCode.OK)]

  [TestCase("nbp", "EUR", "EUR", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("nbp", "PLN", "PLN", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]

  [TestCase("nbp", "QQQ", "EUR", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("nbp", "EUR", "QQQ", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]

  [TestCase("nbp", "EUR", "", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("nbp", "", "USD", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("nbp", "", "", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]

  [TestCase("nbp", "EUR", "USD", $"2026-01-01", "2026-01-01", HttpStatusCode.NoContent)]      //holiday
  [TestCase("nbp", "EUR", "USD", "2026-10-20", "2026-10-20", HttpStatusCode.OK)]              //single working day

  [TestCase("nbp", "EUR", "USD", "2026-01-03", "2026-01-01", HttpStatusCode.BadRequest)]      //dates reversed

  [TestCase("nbp", "EUR", "USD", "2026-01-01", "2026-12-31", HttpStatusCode.OK)]              //whole year

  [TestCase("nbp", "PLN", "PLN", $"2026-01-01", "2026-01-01", HttpStatusCode.BadRequest)]
  [TestCase("nbp", "EUR", "PLN", "", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("nbp", "EUR", "PLN", "2026-01-05", "", HttpStatusCode.OK)]
  [TestCase("nbp", "EUR", "PLN", "", "", HttpStatusCode.NoContent)]

  public async Task GetRates_Nbp(
    string provider,
    string fromCurrency,
    string toCurrency,
    string from,
    string to,
    HttpStatusCode httpCode)
  {
    var requestUri =
        $"/api/v1/Providers/{provider}/rates" +
        $"?fromCurrency={fromCurrency}" +
        $"&toCurrency={toCurrency}" +
        $"&from={from}" +
        $"&to={to}";

    var response = await _client.GetAsync(requestUri);
    Assert.That(response.StatusCode, Is.EqualTo(httpCode));
  }

  [Test]
  [TestCase("frankfurter", "EUR", "PLN", "2026-01-05", "2026-01-09", HttpStatusCode.OK)]
  [TestCase("frankfurter", "PLN", "EUR", "2026-01-05", "2026-01-09", HttpStatusCode.OK)]

  [TestCase("frankfurter", "EUR", "EUR", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "PLN", "PLN", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "QQQ", "EUR", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "EUR", "QQQ", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]

  [TestCase("frankfurter", "EUR", "", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "", "USD", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "", "", "2026-01-05", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "EUR", "USD", $"2026-01-01", "2026-01-01", HttpStatusCode.OK)]             //holiday
  [TestCase("frankfurter", "EUR", "USD", "2026-10-20", "2026-10-20", HttpStatusCode.OK)]              //single working day

  [TestCase("frankfurter", "EUR", "USD", "2026-01-03", "2026-01-01", HttpStatusCode.BadRequest)]      //dates reversed

  [TestCase("frankfurter", "EUR", "USD", "2026-01-01", "2026-12-31", HttpStatusCode.OK)]              //whole year

  [TestCase("frankfurter", "EUR", "USD", "2026-10-25", "2026-10-25", HttpStatusCode.OK)]              //sunday
  [TestCase("frankfurter", "EUR", "PLN", $"2026-01-01", "2026-01-01", HttpStatusCode.OK)]             //holiday
  [TestCase("frankfurter", "EUR", "THB", $"2026-01-01", "2026-01-01", HttpStatusCode.OK)]             //holiday

  [TestCase("frankfurter", "PLN", "PLN", $"2026-01-01", "2026-01-01", HttpStatusCode.BadRequest)]
  [TestCase("test", "EUR", "USD", $"2026-01-01", "2026-01-01", HttpStatusCode.BadRequest)]
  [TestCase("", "EUR", "USD", $"2026-01-01", "2026-01-01", HttpStatusCode.NotFound)]
  [TestCase("frankfurter", "EUR", "PLN", "", "2026-01-09", HttpStatusCode.BadRequest)]
  [TestCase("frankfurter", "EUR", "PLN", "2026-01-05", "", HttpStatusCode.OK)]
  [TestCase("frankfurter", "EUR", "PLN", "", "", HttpStatusCode.OK)]

  public async Task GetRates_Frankfurter(
    string provider,
    string fromCurrency,
    string toCurrency,
    string from,
    string to,
    HttpStatusCode httpCode)
  {

    var requestUri =
        $"/api/v1/Providers/{provider}/rates" +
        $"?fromCurrency={fromCurrency}" +
        $"&toCurrency={toCurrency}" +
        $"&from={from}" +
        $"&to={to}";

    var response = await _client.GetAsync(requestUri);
    Assert.That(response.StatusCode, Is.EqualTo(httpCode));
  }






}
