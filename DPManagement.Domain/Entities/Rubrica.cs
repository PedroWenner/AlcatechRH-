using DPManagement.Domain.Enums;
using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class Rubrica : ISoftDelete
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public TipoRubrica Tipo { get; set; }
    public bool IncideIR { get; set; }
    public bool IncidePrevidencia { get; set; }
    
    // Soft Delete
    public bool Ativo { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
