namespace ExchangeRate.Blazor.WebServiceApi.Dtos;

public sealed class ExchangeRateStatisticsDto
{
    public decimal Min { get; set; }
    public decimal Max { get; set; }
    public decimal Avg { get; set; }
    public int Observations { get; set; }
}
