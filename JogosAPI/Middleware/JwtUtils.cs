using System.IdentityModel.Tokens.Jwt;
using Jogos.ApiService.Middleware.Interface;

namespace Jogos.ApiService.Middleware
{
    public class JwtUtils : IJwtUtils
    {
        private readonly string _issuer;

        private readonly string _secret;
        private readonly int _audience;

        public int? ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;
            try
            {
                // Cria um manipulador de token JWT
                var tokenHandler = new JwtSecurityTokenHandler();

                // Lê o token JWT sem validação
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Busca a claim 'id' (ou 'sub') e tenta convertê-la para inteiro
                var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Id");
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }
            catch
            {
                // Em caso de erro ao ler o token
                return null;
            }

            return null;
        }
    }
}