using ExchangeRate.WebService.Dtos;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using ExchangeRate.WebService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel;

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
      var resolvedProvider = _providerResolver.GetAll()
          .FirstOrDefault(p => p.Code.Equals(provider, StringComparison.OrdinalIgnoreCase));
      if (resolvedProvider is null)
      {
        return BadRequest($"Unsupported provider: {provider}");
      }

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
