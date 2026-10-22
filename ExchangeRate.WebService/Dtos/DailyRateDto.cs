namespace ExchangeRate.WebService.Dtos
{
  public sealed record DailyRateDto(
      DateOnly Date,
      decimal Rate,
      string DisplayDate
    );
}
