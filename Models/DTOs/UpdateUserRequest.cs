using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models.DTOs;

/// <summary>
/// DTO para atualização de usuário
/// Equivalente à validação do update() no UserController do Laravel
/// Todos os campos são opcionais (sometimes)
/// </summary>
public class UpdateUserRequest
{
    [StringLength(255, ErrorMessage = "O nome deve ter no máximo 255 caracteres")]
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "Email inválido")]
    [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
    public string? Email { get; set; }

    [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string? Password { get; set; }
}
