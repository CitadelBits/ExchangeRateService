
using ExchangeRate.WebService.Configuration;
using ExchangeRate.WebService.Consts;
using ExchangeRate.WebService.Extensions;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using ExchangeRate.WebService.Providers.Frankfurter;
using ExchangeRate.WebService.Providers.Nbp;
using Microsoft.Extensions.Options;

namespace ExchangeRate.WebService
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      builder.Services.AddOptions<ExchangeRatesOptions>()
        .Bind(builder.Configuration.GetSection(ConfigurationSections.ExchangeRates))
        .ValidateOnStart();

      // Add services to the container.
      builder.Services.AddSingleton<IValidateOptions<ExchangeRatesOptions>, ExchangeRatesOptionsValidator>();
      builder.Services.AddScoped<IExchangeRateProvider, NbpProvider>();
      builder.Services.AddScoped<IExchangeRateProvider, FrankfurterProvider>();
      builder.Services.AddScoped<IProviderResolver, ProviderResolver>();

      builder.Services.AddControllers();
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddProviderHttpClients(builder.Configuration);


      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();


      app.MapControllers();

      app.Run();
    }
  }
}
