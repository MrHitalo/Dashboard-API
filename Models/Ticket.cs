namespace Dashboard.Models;

/// <summary>
/// Modelo de Ticket
/// Equivalente ao Ticket.php do Laravel
/// </summary>
public class Ticket
{
    public int Id { get; set; }

    public required string Titulo { get; set; }

    public string? Descricao { get; set; }

    public string? Tipo { get; set; }

    public string? Prioridade { get; set; }

    public string? Status { get; set; }

    public int? CriadoPor { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
