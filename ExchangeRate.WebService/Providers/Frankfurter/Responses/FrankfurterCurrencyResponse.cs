using System.Text.Json.Serialization;

namespace ExchangeRate.WebService.Providers.Frankfurter.Responses
{
  internal sealed record FrankfurterCurrencyResponse(
      [property: JsonPropertyName("iso_code")] string IsoCode,
      [property: JsonPropertyName("name")] string Name);
}
