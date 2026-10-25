using CurrencyAggregator.Service.Controllers;
using ExchangeRate.WebService.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.WebService.IntegraionTests
{
  public class ProvidersControllerTests
  {
    private WebApplicationFactory<Program> _factory = null!;
    private IServiceScope _scope = null!;
    private ProvidersController _sut = null!;

    [SetUp]
    public void SetUp()
    {
      _factory = new WebApplicationFactory<Program>();
      _scope = _factory.Services.CreateScope();

      _sut = _scope.ServiceProvider
          .GetRequiredService<ProvidersController>();
    }

    [TearDown]
    public void TearDown()
    {
      _scope.Dispose();
      _factory.Dispose();
    }



    [Test]
    public async Task GetRates_WhenRequestIsValid_ReturnsOk()
    {
      var provider = "nbp";
      var query = new ExchangeRatesQueryDto
      {
        FromCurrency = "EUR",
        ToCurrency = "PLN",
        From = new DateOnly(2026, 1, 5),
        To = new DateOnly(2026, 1, 9)
      };

      using var cancellationTokenSource = new CancellationTokenSource();

      var result = await _sut.GetRates(
          provider,
          query,
          cancellationTokenSource.Token);

      Assert.That(result, Is.InstanceOf<OkObjectResult>());

      var okResult = (OkObjectResult)result;
      Assert.That(okResult.Value, Is.Not.Null);
    }








  }
}
