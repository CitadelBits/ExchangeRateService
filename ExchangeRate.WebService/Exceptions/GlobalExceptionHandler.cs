using ExchangeRate.WebService.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class CustomExceptionHandler : IExceptionHandler
{
  private readonly ILogger<CustomExceptionHandler> _logger;

  public CustomExceptionHandler(
      ILogger<CustomExceptionHandler> logger)
  {
    _logger = logger;
  }

  public async ValueTask<bool> TryHandleAsync(
      HttpContext httpContext,
      Exception exception,
      CancellationToken cancellationToken)
  {
    var (statusCode, title, detail) = exception switch
    {
      UnknownProviderException ex =>
          (StatusCodes.Status400BadRequest,
              "Provider Not Found",
              ex.Message),

      EmptyResponseException ex =>
          (StatusCodes.Status204NoContent,
              "No Data to Process",
              ex.Message),

      ArgumentException ex =>
          (StatusCodes.Status400BadRequest,
              "Invalid Arguments",
              ex.Message),

      InvalidCastException ex =>
                (StatusCodes.Status422UnprocessableEntity,
              "Cannot Proceed",
              ex.Message),

      _ =>
          (StatusCodes.Status500InternalServerError,
              "Internal Server Error",
              "An unexpected server error occurred.")
    };

    if (statusCode >= 500)
    {
      _logger.LogError(
          exception,
          "Unhandled server error.");
    }
    else
    {
      _logger.LogWarning(
          "Request failed with status code {StatusCode}: {Message}",
          statusCode,
          exception.Message);
    }

    var problemDetails = new ProblemDetails
    {
      Status = statusCode,
      Title = title,
      Detail = detail,
      Instance = httpContext.Request.Path
    };

    httpContext.Response.StatusCode = statusCode;

    await httpContext.Response.WriteAsJsonAsync(
        problemDetails,
        cancellationToken);

    return true;
  }
}