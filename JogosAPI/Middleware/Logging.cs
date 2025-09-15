namespace Jogos.ApiService.Middleware
{
    public class Logging
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<Logging> _logger;

        public Logging(RequestDelegate next, ILogger<Logging> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("=============== REQUEST FEITA NO ENDPOINT: {Path}", httpContext.Request.Path.ToString().ToUpper());

            await _next(httpContext);

            _logger.LogInformation("=============== STATUS CODE DA REQUEST: {StatusCode}", httpContext.Response.StatusCode);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Logging>();
        }
    }
}

