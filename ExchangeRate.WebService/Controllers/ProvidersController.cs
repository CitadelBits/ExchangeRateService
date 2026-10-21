using ExchangeRate.WebService.Dtos;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using ExchangeRate.WebService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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

    [HttpGet()]
    [Route("{provider}/currencies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrencies(string provider, CancellationToken cancellationToken)
    {
      var currencies = await _exchangeRateService.GetCurrenciesAsync(
          provider,
          cancellationToken);

      var result = currencies
          .Select(c => new CurrencyDto(c.Code, c.Name))
          .OrderBy(c => c.Code)
          .ToList();

      return Ok(result);
    }

  }
}
