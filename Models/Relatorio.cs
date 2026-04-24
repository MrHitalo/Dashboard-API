namespace Dashboard.Models;

/// <summary>
/// Modelo de Relatório
/// Equivalente ao Relatorio.php do Laravel
/// </summary>
public class Relatorio
{
    public int Id { get; set; }

    public required string Titulo { get; set; }

    public string? Setor { get; set; }

    public string? Conteudo { get; set; }

    public DateTime? DataReferencia { get; set; }

    public string? AutorNome { get; set; }

    public int? CriadoPor { get; set; }

    public string? ArquivoPath { get; set; }

    public string? ArquivoNome { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Relacionamentos

    // Um Relatório tem muitos Arquivos
    public ICollection<RelatorioArquivo> Arquivos { get; set; } = new List<RelatorioArquivo>();
}
