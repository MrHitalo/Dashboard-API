using Dashboard.Models;

namespace Dashboard.Services;

/// <summary>
/// Interface do serviço de geração de tokens JWT
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Gera um token JWT para o usuário
    /// Equivalente ao createToken() do Laravel Sanctum
    /// </summary>
    string GenerateToken(User user);
}
