namespace ExchangeRate.WebService.Models
{
  public sealed record ExchangeRateStatistics(
      decimal Min,
      decimal Max,
      decimal Average,
      int Observations);
}
