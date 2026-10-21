using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.WebService.Dtos
{
  public sealed class ExchangeRatesQueryDto
  {
    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string FromCurrency { get; init; } = string.Empty;

    [Required]
    [StringLength(3, MinimumLength = 3)]
    public string ToCurrency { get; init; } = string.Empty;

    public DateOnly? From { get; init; }

    public DateOnly? To { get; init; }
  }
}
