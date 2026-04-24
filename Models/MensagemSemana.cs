namespace Dashboard.Models;

/// <summary>
/// Modelo de Mensagem da Semana
/// Equivalente ao MensagemSemana.php do Laravel
/// </summary>
public class MensagemSemana
{
    public int Id { get; set; }

    public string? Conteudo { get; set; }

    public string? Responsavel { get; set; }

    public DateTime? SemanaInicio { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
