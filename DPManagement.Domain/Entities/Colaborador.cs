using DPManagement.Domain.Interfaces;

namespace DPManagement.Domain.Entities;

public class Colaborador : ISoftDelete
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string? RG { get; set; }
    public string? PIS { get; set; }
    public DateTime DataNascimento { get; set; }
    public string? Telefone { get; set; }
    public string? Celular { get; set; }
    public string CEP { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty; // UF
    
    public Guid CargoId { get; set; }
    public Cargo Cargo { get; set; } = null!;
    public bool IsDeleted { get; set; } = false;
}
