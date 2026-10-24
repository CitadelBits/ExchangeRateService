using ExchangeRate.WebService.Consts;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using Microsoft.Extensions.Options;
using System.Globalization;

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

      try
      {
        _ = DateTime.Now.ToString(
            options.DateFormat,
            CultureInfo.InvariantCulture);
      }
      catch (FormatException)
      {
        return ValidateOptionsResult.Fail(
            $"Invalid date format: {options.DateFormat}");
      }

      foreach (var provider in options.Providers)
      {
        if(provider.BaseUrl + provider.Endpoints.Currencies != provider.BaseUrl.TrimEnd('/') + '/' + provider.Endpoints.Currencies.TrimStart('/'))
          return ValidateOptionsResult.Fail($"Invalid endpoint configuration for {provider.Code}. Endpoint Currencies");

        if (provider.BaseUrl + provider.Endpoints.Rates != provider.BaseUrl.TrimEnd('/') + '/' + provider.Endpoints.Rates.TrimStart('/'))
          return ValidateOptionsResult.Fail($"Invalid endpoint configuration for {provider.Code}. Endpoint Rates");
      }

      return ValidateOptionsResult.Success;
    }
  
  }
}
