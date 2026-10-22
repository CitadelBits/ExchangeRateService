using System.Text.Json.Serialization;

namespace ExchangeRate.WebService.Providers.Nbp.Responses
{
  internal sealed class NbpRateResponse
  {
    [JsonPropertyName("currency")]
    public string Currency { get; init; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; init; } = string.Empty;

    [JsonPropertyName("mid")]
    public decimal? Mid { get; init; }
  }
}
