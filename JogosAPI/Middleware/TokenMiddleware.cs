namespace Jogos.ApiService.Middleware
{

    using Jogos.ApiService.Middleware.Interface;

    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtUtils jwtUtils;

        public TokenMiddleware(RequestDelegate next, IJwtUtils iJWTUtils)
        {
            _next = next;
            jwtUtils = iJWTUtils;

        }
        public async Task Invoke(HttpContext context)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    var userId = jwtUtils.ValidateToken(token);
                    if (userId != null)
                    {
                        context.Items["Bearer"] = authHeader;
                        context.Items["UserId"] = userId;
                    }
                }
                catch
                {
                    // Token inválido, não faz nada e continua o pipeline
                }
            }
            await _next(context);
        }
    }
}
