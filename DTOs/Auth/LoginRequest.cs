using System.ComponentModel.DataAnnotations;

namespace Dashboard.DTOs.Auth;

/// <summary>
/// DTO para requisição de login
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public required string Password { get; set; }
}
