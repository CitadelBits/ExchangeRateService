using System.Text.Json.Serialization;

namespace ExchangeRate.WebService.Providers.Nbp.Responses
{
  internal sealed record NbpTableResponse(
      [property: JsonPropertyName("table")] string Table,
      [property: JsonPropertyName("no")] string Number,
      [property: JsonPropertyName("effectiveDate")] DateOnly EffectiveDate,
      [property: JsonPropertyName("rates")] List<NbpRateResponse> Rates
  );
}
