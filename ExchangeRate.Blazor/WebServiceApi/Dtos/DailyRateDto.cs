using System.Text.Json.Serialization;

namespace ExchangeRate.Blazor.WebServiceApi.Dtos;

public sealed class DailyRateDto
{
    public DateOnly Date { get; set; }

  [JsonPropertyName("rate")]
   public decimal Value { get; set; }
    public string DisplayDate { get; set; } = "";
}
