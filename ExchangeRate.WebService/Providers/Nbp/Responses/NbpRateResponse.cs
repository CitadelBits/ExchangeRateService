using System.Text.Json.Serialization;

namespace ExchangeRate.WebService.Providers.Nbp.Responses
{
  internal sealed record NbpRateResponse(
      [property: JsonPropertyName("currency")] string Currency,
      [property: JsonPropertyName("code")] string Code,
      [property: JsonPropertyName("mid")] decimal Mid);
}
