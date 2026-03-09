using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class Orgao : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    public int Nivel { get; set; } = 1; // 1 = Orgao, 2 = Secretaria, 3 = Departamento
    
    public Guid? OrgaoPaiId { get; set; }
    public Orgao? OrgaoPai { get; set; }
    
    public ICollection<Orgao> SubOrgaos { get; set; } = new List<Orgao>();
    public ICollection<CentroCusto> CentrosCustos { get; set; } = new List<CentroCusto>();

    public bool IsDeleted { get; set; } = false;
    public bool Ativo { get; set; } = true;
}
