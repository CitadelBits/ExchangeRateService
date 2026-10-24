using ExchangeRate.Blazor.WebServiceApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRate.Blazor.WebServiceApi;

public class ExchangeRateClient : IDisposable
{
    // Zmień port na adres HTTPS/HTTP uruchomionego backendu.
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("https://localhost:7064/")
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ProvidersResponseDto> GetProvidersAsync(
        CancellationToken cancellationToken = default)
    {
        return await GetFromServiceAsync<ProvidersResponseDto>(
            "api/v1/providers", cancellationToken);
    }

    public async Task<List<CurrencyDto>> GetCurrenciesAsync(
        string provider,
        CancellationToken cancellationToken = default)
    {
        return await GetFromServiceAsync<List<CurrencyDto>>(
            $"api/v1/providers/{Uri.EscapeDataString(provider)}/currencies",
            cancellationToken);
    }

    public async Task<List<DailyRateDto>> GetRatesAsync(
        string provider,
        string fromCurrency,
        string toCurrency,
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default)
    {
    var url = $"api/v1/providers/{Uri.EscapeDataString(provider)}/rates" +
              $"?fromCurrency={Uri.EscapeDataString(fromCurrency)}" +
              $"&toCurrency={Uri.EscapeDataString(toCurrency)}";
    if (from.HasValue)
      url += $"&from={from.Value:yyyy-MM-dd}";

    if (to.HasValue)
      url += $"&to={to.Value:yyyy-MM-dd}";

    return await GetFromServiceAsync<List<DailyRateDto>>(url, cancellationToken);
    }

    public async Task<ExchangeRateStatisticsDto> GetStatisticsAsync(
        string provider,
        string fromCurrency,
        string toCurrency,
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken = default)
    {
    var url = $"api/v1/providers/{Uri.EscapeDataString(provider)}/statistics" +
              $"?fromCurrency={Uri.EscapeDataString(fromCurrency)}" +
              $"&toCurrency={Uri.EscapeDataString(toCurrency)}";

    if (from.HasValue)
          url += $"&from={from.Value:yyyy-MM-dd}";

    if (to.HasValue)
      url += $"&to={to.Value:yyyy-MM-dd}";

    return await GetFromServiceAsync<ExchangeRateStatisticsDto>(url, cancellationToken);
    }

  private async Task<T> GetFromServiceAsync<T>(
      string url,
      CancellationToken cancellationToken)
  {
    using var response = await _httpClient.GetAsync(url, cancellationToken);
    var body = await response.Content.ReadAsStringAsync(cancellationToken);

    if (!response.IsSuccessStatusCode)
    {

      var details = JsonSerializer.Deserialize<ProblemDetails>(body);

      string detailedMessage = details != null ? $"{details.Title}: {details.Detail}" : body;
      throw new HttpRequestException(
                $"API returned {(int)response.StatusCode} ({response.ReasonPhrase}){Environment.NewLine}{detailedMessage}",
                null,
                response.StatusCode);
    }



    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
    {
      throw new HttpRequestException(
          $"API returned {(int)response.StatusCode} ({response.ReasonPhrase})",
          null,
          response.StatusCode);
    }

    var result = JsonSerializer.Deserialize<T>(body, JsonOptions);
    return result ?? throw new InvalidOperationException(
        "API returned an empty or null response.");
  }

    public void Dispose() => _httpClient.Dispose();
}
