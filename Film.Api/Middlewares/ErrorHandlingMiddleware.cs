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
        catch(BadHttpRequestException bhrx)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(new { message = bhrx.Message });
            await context.Response.WriteAsync(json);
        }
        catch(Exception ex)
        {
            var real = ex is AggregateException ag
               ? ag.InnerException ?? ag
               : ex;

            // 2. if the real exception is the one we want, treat it as 400
            if (real is BadHttpRequestException bad)
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = bad.Message }));
                return;
            }
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            string json = JsonSerializer.Serialize(new { message = ex.Message });
            await context.Response.WriteAsync(json);
        }
    }
}
