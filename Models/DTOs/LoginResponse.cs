namespace Dashboard.Models.DTOs;

/// <summary>
/// DTO para resposta de login
/// Equivalente ao JSON retornado no login() do Laravel
/// </summary>
public class LoginResponse
{
    public required string Message { get; set; }
    public required UserDto User { get; set; }
    public required string Token { get; set; }
}

/// <summary>
/// DTO de usuário (sem informações sensíveis como senha)
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}
