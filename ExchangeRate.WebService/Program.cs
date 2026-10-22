
using ExchangeRate.WebService.Configuration;
using ExchangeRate.WebService.Consts;
using ExchangeRate.WebService.Extensions;
using ExchangeRate.WebService.Options;
using ExchangeRate.WebService.Providers;
using ExchangeRate.WebService.Providers.Frankfurter;
using ExchangeRate.WebService.Providers.Nbp;
using Microsoft.Extensions.Options;
using System.Reflection;

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
      builder.Services.AddExchangeRateServices();
      builder.Services.AddProviderHttpClients(builder.Configuration);

      builder.Services.AddProblemDetails();
      builder.Services.AddExceptionHandler<CustomExceptionHandler>();

      builder.Services.AddControllers();
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen(c =>
      {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });


      var app = builder.Build();

      // Configure the HTTP request pipeline.
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseAuthorization();

      app.UseExceptionHandler();

      app.MapControllers();

      app.Run();
    }
  }
}
