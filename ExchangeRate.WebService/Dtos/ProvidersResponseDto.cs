namespace ExchangeRate.WebService.Dtos
{
  public sealed record ProvidersResponseDto(
      string DefaultProvider,
      IReadOnlyList<ProviderDto> Providers);
}
