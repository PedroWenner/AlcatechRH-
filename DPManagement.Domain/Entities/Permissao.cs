using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class Permissao : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Modulo { get; set; } = string.Empty; // ex: Folha, Funcionario
    public string? ModuloPai { get; set; } // ex: Cadastros
    public string Acao { get; set; } = string.Empty;   // ex: Ler, Calcular, Editar
    public string? Descricao { get; set; }

    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; }

    // Relacionamentos
    public ICollection<PerfilPermissao> PerfilPermissoes { get; set; } = new List<PerfilPermissao>();
}
