namespace DPManagement.Domain.Entities;

public class Banco
{
    public Guid Id { get; set; }
    public string CodigoBanco { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string NomeCurto { get; set; } = string.Empty;
}
