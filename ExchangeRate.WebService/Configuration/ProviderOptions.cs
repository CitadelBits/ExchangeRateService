namespace ExchangeRate.WebService.Options
{
  public sealed class ProviderOptions
  {
    public string Code { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string BaseUrl { get; set; } = "";
    public int TimeoutSeconds { get; set; } = 10;

    public ProviderEndpoints Endpoints { get; set; } = new();

    public Dictionary<string, string> Settings { get; set; } = [];
  }
}
