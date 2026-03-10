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
    public bool Ativo { get; set; }
}

public class RubricaCreateUpdateDto
{
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoRubrica Tipo { get; set; }
    public bool IncideIR { get; set; }
    public bool IncidePrevidencia { get; set; }
}
