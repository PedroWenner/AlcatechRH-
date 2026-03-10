using DPManagement.Domain.Enums;

namespace DPManagement.Application.DTOs;

public class VinculoDto
{
    public Guid Id { get; set; }
    
    public Guid ColaboradorId { get; set; }
    public string ColaboradorNome { get; set; } = string.Empty;
    public string ColaboradorCpf { get; set; } = string.Empty;
    
    public Guid OrgaoId { get; set; }
    public string OrgaoNome { get; set; } = string.Empty;
    public string OrgaoAbreviatura { get; set; } = string.Empty;
    
    public string Matricula { get; set; } = string.Empty;
    
    public Guid CargoId { get; set; }
    public string CargoNome { get; set; } = string.Empty;
    
    public RegimeJuridico RegimeJuridicoId { get; set; }
    public string RegimeJuridicoDescricao { get; set; } = string.Empty;
    
    public FormaIngresso FormaIngressoId { get; set; }
    public string FormaIngressoDescricao { get; set; } = string.Empty;
    
    public Guid CentroCustoId { get; set; }
    public string CentroCustoDescricao { get; set; } = string.Empty;
    
    public DateTime DataAdmissao { get; set; }
    
    public bool Ativo { get; set; }
}

public class VinculoCreateUpdateDto
{
    public Guid ColaboradorId { get; set; }
    public Guid OrgaoId { get; set; }
    public string Matricula { get; set; } = string.Empty;
    public Guid CargoId { get; set; }
    public RegimeJuridico RegimeJuridicoId { get; set; }
    public FormaIngresso FormaIngressoId { get; set; }
    public Guid CentroCustoId { get; set; }
    public DateTime DataAdmissao { get; set; }
}
