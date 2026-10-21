using ExchangeRate.WebService.Consts;
using ExchangeRate.WebService.Options;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService.Extensions
{
  public static class HttpClientConfigurationExtensions
  {
    public static IServiceCollection AddProviderHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
      var providers = configuration
          .GetSection(ConfigurationSections.ExchangeRates)
          .Get<ExchangeRatesOptions>()?
          .Providers ?? [];

      foreach (var provider in providers)
      {
        services.AddHttpClient(provider.Code, client =>
        {
          client.BaseAddress = new Uri(provider.BaseUrl);
          client.Timeout = TimeSpan.FromSeconds(
              provider.TimeoutSeconds);
        });
      }

      return services;
    }
  }
}
