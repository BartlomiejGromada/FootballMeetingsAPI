using Domain.Exceptions;

namespace Web.Middlewares
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            await context.Response.WriteAsJsonAsync(new
            {
                error = exception switch
                {
                    NotFoundException => exception.Message,
                    _ => "Something went wrong",
                },
            });
        }
    }
}
