using System.Text.Json.Serialization;

namespace ExchangeRate.WebService.Providers.Frankfurter.Responses
{
  internal sealed record FrankfurterRateResponse(
      [property: JsonPropertyName("date")] DateOnly Date,
      [property: JsonPropertyName("base")] string BaseCurrency,
      [property: JsonPropertyName("quote")] string TargetCurrency,
      [property: JsonPropertyName("rate")] decimal Rate);
}
