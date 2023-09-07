using System.Net.Mime;
using Application.Common.Exceptions;

namespace WebApi.Middlewares.ExceptionHandling;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (AbstractHttpException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = MediaTypeNames.Text.Plain;
            if (ex.Message != null) await context.Response.WriteAsync(ex.Message);
        }
    }
}