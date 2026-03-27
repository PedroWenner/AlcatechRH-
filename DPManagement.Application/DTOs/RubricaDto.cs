using DPManagement.Domain.Enums;

namespace DPManagement.Application.DTOs;

public class RubricaDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoRubrica Tipo { get; set; }
    public string TipoDescricao { get; set; } = string.Empty;
    public bool IncideIR { get; set; }
    public bool IncidePrevidencia { get; set; }
    public RotinaCalculo Rotina { get; set; }
    public string RotinaDescricao { get; set; } = string.Empty;
    public bool Ativo { get; set; }

    // eSocial S-1010
    public string? NatRubr { get; set; }
    public string? IdeTabRubr { get; set; }
    public string? CodIncCP { get; set; }
    public string? CodIncIRRF { get; set; }
    public string? CodIncFGTS { get; set; }
    public string? CodIncPisPasep { get; set; }
    public string IniValid { get; set; } = string.Empty;
    public string? FimValid { get; set; }
}

public class RubricaCreateUpdateDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoRubrica Tipo { get; set; }
    public bool IncideIR { get; set; }
    public bool IncidePrevidencia { get; set; }
    public RotinaCalculo Rotina { get; set; }

    // eSocial S-1010
    public string? NatRubr { get; set; }
    public string? IdeTabRubr { get; set; }
    public string? CodIncCP { get; set; }
    public string? CodIncIRRF { get; set; }
    public string? CodIncFGTS { get; set; }
    public string? CodIncPisPasep { get; set; }
    public string IniValid { get; set; } = string.Empty;
    public string? FimValid { get; set; }
}
