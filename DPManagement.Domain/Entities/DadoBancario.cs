using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class DadoBancario : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string CodigoBanco { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string DigitoAgencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
    public string DigitoConta { get; set; } = string.Empty;
    public string Operacao { get; set; } = string.Empty;

    public Guid ColaboradorId { get; set; }
    public Colaborador Colaborador { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;
}
