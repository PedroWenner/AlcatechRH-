using DPManagement.Domain.Enums;
using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class Vinculo : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid ColaboradorId { get; set; }
    public Colaborador Colaborador { get; set; } = null!;
    
    public Guid OrgaoId { get; set; }
    public Orgao Orgao { get; set; } = null!;
    
    public string Matricula { get; set; } = string.Empty;
    
    public Guid CargoId { get; set; }
    public Cargo Cargo { get; set; } = null!;
    
    public RegimeJuridico RegimeJuridicoId { get; set; }
    
    public FormaIngresso FormaIngressoId { get; set; }
    
    public Guid CentroCustoId { get; set; }
    public CentroCusto CentroCusto { get; set; } = null!;
    
    public DateTime DataAdmissao { get; set; }
    
    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
