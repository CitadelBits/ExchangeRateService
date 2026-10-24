using System.ComponentModel.DataAnnotations;

namespace ExchangeRate.Blazor.Models
{
  public class ExchangeRatesFilter
  {
    [Required(ErrorMessage = "Wybierz dostawcę")]
    public string Provider { get; set; }

    [Required(ErrorMessage = "Wybierz walutę źródłową.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Kod waluty musi mieć dokładnie 3 znaki.")]
    public string FromCurrency { get; set; }

    [Required(ErrorMessage = "Wybierz walutę docelową.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Kod waluty musi mieć dokładnie 3 znaki.")]
    public string ToCurrency { get; set; }

    public DateOnly? From { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? To { get; set; } = DateOnly.FromDateTime(DateTime.Now);
  }
}
