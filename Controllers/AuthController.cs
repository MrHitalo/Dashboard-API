using Dashboard.Data;
using Dashboard.Models;
using Dashboard.Models.DTOs;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers;

/// <summary>
/// Controller de autenticação - Equivalente ao AuthController.php do Laravel
/// </summary>
[ApiController]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthController(AppDbContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Cadastro de usuário
    /// POST /api/register
    /// Equivalente ao register() do Laravel
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // Validação automática via Data Annotations
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Verifica se o email já existe
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest(new { message = "Email já cadastrado" });
        }

        // Hash da senha usando BCrypt
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Cria o usuário
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Retorna sem a senha (segurança)
        var userDto = new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return CreatedAtAction(nameof(Register), new
        {
            user = userDto,
            mensagem = "Usuario criado com sucesso!"
        });
    }

    /// <summary>
    /// Login de usuário
    /// POST /api/login
    /// Equivalente ao login() do Laravel
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Busca o usuário pelo email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        // Verifica se o usuário existe e se a senha está correta
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
        {
            return Unauthorized(new { message = "Credenciais inválidas" });
        }

        // Gera o token JWT (equivalente ao createToken do Sanctum)
        var token = _jwtService.GenerateToken(user);

        // Retorna a resposta
        var response = new LoginResponse
        {
            Message = "Login realizado com sucesso",
            User = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            },
            Token = token
        };

        return Ok(response);
    }

    /// <summary>
    /// Logout de usuário
    /// POST /api/logout
    /// Equivalente ao logout() do Laravel
    /// Requer autenticação (token JWT)
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        // Em JWT stateless, o logout é feito no frontend (removendo o token)
        // Aqui apenas retornamos sucesso
        // Para invalidar tokens, precisaria de uma blacklist (implementação futura)

        return Ok(new { message = "Logout realizado com sucesso" });
    }
}
