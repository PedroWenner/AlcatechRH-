using System.ComponentModel;

namespace DPManagement.Domain.Enums;

public enum RotinaCalculo
{
    [Description("Outros")]
    Outros = 0,

    [Description("Salário Base")]
    SalarioBase = 1,

    [Description("INSS")]
    INSS = 2,

    [Description("IRRF")]
    IRRF = 3,

    [Description("ATS (Anuênio/Triênio/Quinquênio)")]
    ATS = 4
}
