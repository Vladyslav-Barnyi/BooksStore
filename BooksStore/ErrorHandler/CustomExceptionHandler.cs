using System.Net;
using System.Text.Json;

namespace BooksStore.ErrorHandler
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(RequestDelegate next, ILogger<CustomExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception: {Exception}", ex);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            _logger.LogError($"Exception Occurred: {exception}");
            
            
            var message = exception switch
            {
                BadHttpRequestException => "The call is incorrectly formatted X",
                UnauthorizedAccessException => "You can't access the resource  Y",
                _ => "An error has occurred Z"
            };
            
            var response = new
            {
                description = message,
                detail = exception.Message,
                correlationId = Guid.NewGuid()
            };

            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
