using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class Perfil : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    
    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; }
    
    // Relacionamentos
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<PerfilPermissao> PerfilPermissoes { get; set; } = new List<PerfilPermissao>();
}
