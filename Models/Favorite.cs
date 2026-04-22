namespace Dashboard.Models;

/// <summary>
/// Modelo de favorito - Equivalente ao Favorite.php do Laravel
/// Relaciona usuários com produtos favoritos
/// </summary>
public class Favorite
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public User User { get; set; } = null!;
    public Product Product { get; set; } = null!;
}
