using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class CentroCusto : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Descricao { get; set; } = string.Empty;

    public Guid OrgaoId { get; set; }
    public Orgao? Orgao { get; set; }
    public ICollection<Vinculo> Vinculos { get; set; } = new List<Vinculo>();

    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
