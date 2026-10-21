namespace ExchangeRate.WebService.Dtos
{
  public sealed record ExchangeRateStatisticsDto(
      decimal Min,
      decimal Max,
      decimal Average,
      int Observations);
}
