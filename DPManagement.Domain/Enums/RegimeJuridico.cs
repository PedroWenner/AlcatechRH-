using System.ComponentModel;

namespace DPManagement.Domain.Enums;

public enum RegimeJuridico
{
    [Description("Estatutário")]
    Estatutario = 1,
    
    [Description("Celetista (CLT)")]
    Celetista = 2,
    
    [Description("Cargo Comissionado")]
    CargoComissionado = 3,
    
    [Description("Contrato Temporário (PSS)")]
    ContratoTemporario = 4,
    
    [Description("Estagiário")]
    Estagiario = 5,
    
    [Description("Jovem Aprendiz")]
    JovemAprendiz = 6
}
