namespace ExchangeRate.WebService.Dtos
{
  public sealed record ExchangeRatesResponseDto(
      string Provider,
      CurrencyDto FromCurrency,
      CurrencyDto ToCurrency,
      DateOnly From,
      DateOnly To,
      ExchangeRateStatisticsDto Statistics,
      IReadOnlyList<DailyRateDto> Rates);
}
