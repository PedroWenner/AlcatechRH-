namespace DPManagement.Domain.Entities;

public class Perfil
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    
    // Relacionamentos
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<PerfilPermissao> PerfilPermissoes { get; set; } = new List<PerfilPermissao>();
}
