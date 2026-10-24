using ExchangeRate.WebService.Dtos;
using ExchangeRate.WebService.Models;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using ExchangeRate.WebService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace CurrencyAggregator.Service.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class ProvidersController : ControllerBase
  {
    private readonly ILogger<ProvidersController> _logger;
    private IProviderResolver _providerResolver;
    private readonly IOptions<ExchangeRatesOptions> _options;
    private readonly IExchangeRateService _exchangeRateService;

    public ProvidersController(ILogger<ProvidersController> logger, 
      IProviderResolver providerResolver, 
      IOptions<ExchangeRatesOptions> options,
      IExchangeRateService exchangeRateService
      )
    {
      _logger = logger;
      _providerResolver = providerResolver;
      _options = options;
      _exchangeRateService = exchangeRateService;
    }


    /// <summary>
    /// Get all available providers and the default provider.
    /// </summary>
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public Task<IActionResult> GetProviders()
    {
      var providers = _providerResolver.GetAll()
      .Select(providers => new ProviderDto(providers.Code,providers.DisplayName))
        .ToList();

      var response = new ProvidersResponseDto(
          _options.Value.DefaultProvider,
          providers);

      return Task.FromResult<IActionResult>(Ok(response));
    }


    /// <summary>
    /// Get currencies from the specified provider.
    /// </summary>
    /// <param name="provider">Provider code, e.g. nbp or frankfurter</param>
    [HttpGet()]
    [Route("{provider}/currencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCurrencies(string provider, 
      CancellationToken cancellationToken)
    {
      var currencies = await _exchangeRateService.GetCurrenciesAsync(
          provider,
          cancellationToken);

      var result = currencies
          .Select(c => new CurrencyDto(c.Code, c.Name))
          .ToList();

      return Ok(result);
    }

    /// <summary>
    /// Get rates from the specified provider.
    /// </summary>
    /// <param name="provider">Provider code, e.g. nbp or frankfurter</param>
    /// <param name="query">Exchange rates query parameters</param>
    [HttpGet()]
    [Route("{provider}/rates")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRates(
        string provider,
        [FromQuery] ExchangeRatesQueryDto query,
        CancellationToken cancellationToken)
    {
      var rates = await _exchangeRateService.GetRatesAsync(
          provider,
          query.FromCurrency,
          query.ToCurrency,
          query.From,
          query.To,
          cancellationToken);

      var result = rates
          .Select(
          r => new DailyRateDto(
            r.Date, 
            r.Value,
            r.Date.ToString(_options.Value.DateFormat, CultureInfo.CurrentCulture),
            $"1 {r.BaseCurrency} = {r.Value} {r.TargetCurrency}"
            )
          ).ToList();

      return Ok(result);
    }

    /// <summary>
    /// Get statistics from the specified provider.
    /// </summary>
    [HttpGet()]
    [Route("{provider}/statistics")]
    public async Task<ActionResult<ExchangeRateStatistics>> GetStatistics(
        string provider,
        [FromQuery] ExchangeRatesQueryDto query,
        CancellationToken cancellationToken)
    {
      var statistics = await _exchangeRateService.GetStatisticsAsync(
          provider,
          query.FromCurrency,
          query.ToCurrency,
          query.From,
          query.To,
          cancellationToken);

      return Ok(statistics);
    }

  }
}
