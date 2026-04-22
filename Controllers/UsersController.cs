using Dashboard.Data;
using Dashboard.Models;
using Dashboard.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dashboard.Controllers;

/// <summary>
/// Controller de usuários - Equivalente ao UserController.php do Laravel
/// Todas as rotas requerem autenticação (middleware auth:sanctum)
/// </summary>
[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém o ID do usuário autenticado a partir do token JWT
    /// </summary>
    private int GetAuthenticatedUserId()
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    /// <summary>
    /// Exibir um usuário específico
    /// GET /api/users/{id}
    /// Equivalente ao show() do Laravel
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Show(int id)
    {
        var authenticatedUserId = GetAuthenticatedUserId();

        // Verifica se o usuário autenticado está tentando acessar seus próprios dados
        if (authenticatedUserId != id)
        {
            return Forbid(); // 403 Forbidden
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { mensagem = "Usuário não encontrado" });
        }

        // Retorna sem a senha
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return Ok(new { user = userDto });
    }

    /// <summary>
    /// Atualizar um usuário
    /// PUT /api/users/{id}
    /// Equivalente ao update() do Laravel
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserRequest request)
    {
        var authenticatedUserId = GetAuthenticatedUserId();

        // Verifica se o usuário autenticado está tentando atualizar seus próprios dados
        if (authenticatedUserId != id)
        {
            return StatusCode(403, new { mensagem = "tentativa inválida" });
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { mensagem = "Usuário não encontrado" });
        }

        // Atualiza apenas os campos fornecidos (similar ao sometimes do Laravel)
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            user.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            user.Email = request.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        // Retorna sem a senha
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return Ok(new { user = userDto });
    }

    /// <summary>
    /// Deletar um usuário
    /// DELETE /api/users/{id}
    /// Equivalente ao destroy() do Laravel
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Destroy(int id)
    {
        var authenticatedUserId = GetAuthenticatedUserId();

        // Verifica se o usuário autenticado está tentando deletar sua própria conta
        if (authenticatedUserId != id)
        {
            return StatusCode(403, new { mensagem = "tentativa inválida" });
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound(new { mensagem = "Usuário não encontrado" });
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { mensagem = "Usuário apagado com sucesso!" });
    }
}
