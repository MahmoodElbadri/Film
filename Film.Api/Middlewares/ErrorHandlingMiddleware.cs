using Film.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Film.Api.Middlewares;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException nfx)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(new { message = nfx.Message });
            await context.Response.WriteAsync(json);
        }
        catch(Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(new { message = ex.Message });
            await context.Response.WriteAsync(json);
        }
    }
}
