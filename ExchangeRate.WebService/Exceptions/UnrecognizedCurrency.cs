namespace ExchangeRate.WebService.Exceptions
{
  public class UnrecognizedCurrency : Exception
  {
    public UnrecognizedCurrency(string message)
      : base(message)
    {
    }
  }
}
