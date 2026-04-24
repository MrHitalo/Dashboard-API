namespace Dashboard.Models;

/// <summary>
/// Modelo de Estoque
/// Equivalente ao Estoque.php do Laravel
/// </summary>
public class Estoque
{
    public int Id { get; set; }

    public required string Nome { get; set; }

    public string? Codigo { get; set; }

    public string? Categoria { get; set; }

    public int Quantidade { get; set; }

    public int? QuantidadeMinima { get; set; }

    public string? Unidade { get; set; }

    public string? Localizacao { get; set; }

    public decimal? Preco { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
