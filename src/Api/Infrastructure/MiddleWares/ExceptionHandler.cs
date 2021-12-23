using System.Net;
using System.Net.Mime;

namespace Api.Infrastructure.MiddleWares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            await HandleException(httpContext, exception);
        }
    }

    private async Task HandleException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = (int) Parse(exception);
        await httpContext.Response.WriteAsJsonAsync(exception.Message);
    }

    private static HttpStatusCode Parse(Exception exception)
    {
        return exception switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.Conflict,
            _ => HttpStatusCode.InternalServerError
        };
    }
}