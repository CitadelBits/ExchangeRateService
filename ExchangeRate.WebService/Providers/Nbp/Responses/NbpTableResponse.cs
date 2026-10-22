using System.Text.Json.Serialization;

namespace ExchangeRate.WebService.Providers.Nbp.Responses
{
  internal sealed class NbpTableResponse
  {
    [JsonPropertyName("table")]
    public string Table { get; init; } = string.Empty;

    [JsonPropertyName("no")]
    public string Number { get; init; } = string.Empty;

    [JsonPropertyName("effectiveDate")]
    public DateOnly EffectiveDate { get; init; }

    [JsonPropertyName("rates")]
    public List<NbpRateResponse> Rates { get; init; } = [];
  }
}
