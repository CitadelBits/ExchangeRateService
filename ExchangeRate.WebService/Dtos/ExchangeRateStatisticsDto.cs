namespace ExchangeRate.WebService.Dtos
{
  public sealed record ExchangeRateStatisticsDto(
      decimal Min,
      decimal Max,
      decimal Avg,
      int Observations);
}
