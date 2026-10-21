using ExchangeRate.WebService.Consts;
using ExchangeRate.WebService.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.Configuration
{
  public class ExchangeRatesOptionsValidator : IValidateOptions<ExchangeRatesOptions>
  {
    public ValidateOptionsResult Validate(string? name, ExchangeRatesOptions options)
    {
      bool isValid = options.Providers.Any(p => p.Code == options.DefaultProvider);
      if (!isValid)
      {
        return ValidateOptionsResult.Fail($"Unrecognized default provider: {options.DefaultProvider}");
      }
      return ValidateOptionsResult.Success;
    }
  
  }
}
