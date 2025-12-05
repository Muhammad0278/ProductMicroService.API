

namespace eCommerce.ProductMicroService.API.Middleware;

// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class ExceptionHandlingMidleware
{
    private readonly ILogger<ExceptionHandlingMidleware> _logger;   
    private readonly RequestDelegate _next;

    public ExceptionHandlingMidleware(RequestDelegate next,ILogger<ExceptionHandlingMidleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            //logging
            _logger.LogError($"{ex.GetType().ToString()} : {ex.Message}");

            if (ex.InnerException is not null)
            {
                _logger.LogError($"{ex.InnerException.GetType().ToString()} : {ex.InnerException.Message}");
            }
            httpContext.Response.StatusCode = 500;
            // internal server error
            await httpContext.Response.WriteAsJsonAsync(new
            {

                Message = ex.Message,
                Type = ex.GetType().ToString()
            });
        }

    }
}

// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlingMidlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMidleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMidleware>();
    }
}
