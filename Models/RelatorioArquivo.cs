namespace Dashboard.Models;

/// <summary>
/// Modelo de Arquivo de Relatório
/// Equivalente ao RelatorioArquivo.php do Laravel
/// </summary>
public class RelatorioArquivo
{
    public int Id { get; set; }

    public int RelatorioId { get; set; }

    public string? ArquivoPath { get; set; }

    public string? ArquivoNome { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamento: RelatorioArquivo pertence a um Relatorio
    public Relatorio Relatorio { get; set; } = null!;
}
