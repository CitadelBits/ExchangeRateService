namespace ExchangeRate.WebService.Exceptions
{
  public sealed class UnknownProviderException : Exception
  {
    public UnknownProviderException(string providerCode)
        : base($"Unsupported provider: {providerCode}")
    {
    }
  }
}
