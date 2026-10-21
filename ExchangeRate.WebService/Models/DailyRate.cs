namespace ExchangeRate.WebService.Models
{
  public sealed record DailyRate(
      DateOnly Date,
      string BaseCurrency,
      string TargetCurrency,
      decimal Value);
}
