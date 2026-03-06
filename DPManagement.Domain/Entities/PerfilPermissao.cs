namespace DPManagement.Domain.Entities;

public class PerfilPermissao
{
    public Guid PerfilId { get; set; }
    public Perfil Perfil { get; set; } = null!;

    public Guid PermissaoId { get; set; }
    public Permissao Permissao { get; set; } = null!;
}
