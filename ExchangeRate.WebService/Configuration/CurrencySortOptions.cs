namespace ExchangeRate.WebService.Options
{
  public sealed class CurrencySortOptions
  {
    public SortField Field { get; set; } = SortField.Code;
    public SortDirection Direction { get; set; } = SortDirection.Ascending;
  }
}
