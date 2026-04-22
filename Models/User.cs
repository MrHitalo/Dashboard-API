namespace Dashboard.Models;

/// <summary>
/// Modelo de usuário - Equivalente ao User.php do Laravel
/// </summary>
public class User
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public DateTime? EmailVerifiedAt { get; set; }

    public string? RememberToken { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamento: Um usuário tem muitos favoritos
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
