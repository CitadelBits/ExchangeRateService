using ExchangeRate.WebService.Consts;

namespace ExchangeRate.WebService.Options
{
  public sealed class ExchangeRatesOptions
  {
    public string DateFormat { get; set; } = "yyyy-MM-dd";
    public string DefaultProvider { get; set; } = ApplicationConstants.Nbp;

    public CurrencySortOptions CurrencySort { get; set; } = new();

    public List<ProviderOptions> Providers { get; set; } = [];
  }
}
