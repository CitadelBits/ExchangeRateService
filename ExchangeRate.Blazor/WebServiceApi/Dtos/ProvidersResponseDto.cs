namespace ExchangeRate.Blazor.WebServiceApi.Dtos
{
  public sealed record ProvidersResponseDto(
      string DefaultProvider,
      IReadOnlyList<ProviderDto> Providers);
}
