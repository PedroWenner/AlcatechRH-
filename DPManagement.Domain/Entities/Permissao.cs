namespace DPManagement.Domain.Entities;

public class Permissao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Modulo { get; set; } = string.Empty; // ex: Folha, Funcionario
    public string Acao { get; set; } = string.Empty;   // ex: Ler, Calcular, Editar
    public string? Descricao { get; set; }

    // Relacionamentos
    public ICollection<PerfilPermissao> PerfilPermissoes { get; set; } = new List<PerfilPermissao>();
}
