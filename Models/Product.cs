namespace Dashboard.Models;

/// <summary>
/// Modelo de produto - Equivalente ao Product.php do Laravel
/// </summary>
public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamento: Um produto tem muitos favoritos
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
