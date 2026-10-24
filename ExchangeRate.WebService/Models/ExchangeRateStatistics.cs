namespace ExchangeRate.WebService.Models
{
  public sealed record ExchangeRateStatistics(
      decimal Min,
      decimal Max,
      decimal Avg,
      int Observations);
}
