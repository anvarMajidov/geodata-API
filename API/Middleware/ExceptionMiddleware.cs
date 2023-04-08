using System.Net;
using System.Text.Json;
using API.Shared;

namespace API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            var statusCode = HttpStatusCode.InternalServerError;
            if (ex is HttpRequestException exception) statusCode = exception.StatusCode ?? statusCode;
            
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) statusCode;
            
            var response = new ApiException 
            {
                StatusCode = statusCode,
                Message = ex.Message
            };
            
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        
            var json = JsonSerializer.Serialize(response, options);
        
            await context.Response.WriteAsync(json);
        }
    }
}
