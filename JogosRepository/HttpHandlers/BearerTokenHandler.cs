using Microsoft.AspNetCore.Http;

namespace Jogos.Service.Infrastructure.HttpHandlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAcessor;

        public BearerTokenHandler(IHttpContextAccessor httpContextAcessor)
        {
            _httpContextAcessor = httpContextAcessor;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var context = _httpContextAcessor.HttpContext;
            if ((context != null && context.Items.ContainsKey("Bearer")))
            {
                var token = context.Items["Bearer"] as string;
                if(!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Substring("Bearer ".Length).Trim());
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
