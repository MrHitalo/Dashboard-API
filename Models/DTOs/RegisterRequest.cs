using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models.DTOs;

/// <summary>
/// DTO para requisição de registro de usuário
/// Equivalente à validação do register() no AuthController do Laravel
/// </summary>
public class RegisterRequest
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public required string Password { get; set; }
}
