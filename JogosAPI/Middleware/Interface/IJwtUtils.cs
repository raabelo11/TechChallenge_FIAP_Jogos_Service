namespace Jogos.ApiService.Middleware.Interface
{
    public interface IJwtUtils
    {
        int? ValidateToken(string token);
    }
}
