using System.Net;
using System.Text.Json;

namespace BookingApp.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (HttpRequestException)
        {
            await WriteErrorAsync(context, HttpStatusCode.ServiceUnavailable, "The Rooms API is unavailable.");
        }
        catch (TimeoutException)
        {
            await WriteErrorAsync(context, HttpStatusCode.ServiceUnavailable, "The Rooms API did not respond in time.");
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
        }
        catch (Exception)
        {
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode,string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var ressponse = new
        {
            statusCode = (int)statusCode,
            message
        };

        var json = JsonSerializer.Serialize(ressponse);
        await context.Response.WriteAsync(json);
    }

}
