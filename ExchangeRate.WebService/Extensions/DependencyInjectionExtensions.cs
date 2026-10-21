using ExchangeRate.WebService.Configuration;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using ExchangeRate.WebService.Providers.Frankfurter;
using ExchangeRate.WebService.Providers.Nbp;
using ExchangeRate.WebService.Services;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.Extensions
{
  public static class DependencyInjectionExtensions
  {
    public static IServiceCollection AddExchangeRateServices(
        this IServiceCollection services)
    {
      services.AddSingleton<IValidateOptions<ExchangeRatesOptions>, ExchangeRatesOptionsValidator>();
      services.AddScoped<IExchangeRateProvider, NbpProvider>();
      services.AddScoped<IExchangeRateProvider, FrankfurterProvider>();
      services.AddScoped<IProviderResolver, ProviderResolver>();
      services.AddScoped<IExchangeRateService, ExchangeRateService>();

      return services;
    }
  }
}
