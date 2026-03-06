namespace DPManagement.Application.DTOs;

public class ColaboradorDto
{
    public Guid Id { get; set; }
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
    public string Estado { get; set; } = string.Empty;
    public Guid CargoId { get; set; }
    public string CargoNome { get; set; } = string.Empty;
}

public class CreateColaboradorDto
{
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
    public string Estado { get; set; } = string.Empty;
    public Guid CargoId { get; set; }
}

public class UpdateColaboradorDto : CreateColaboradorDto
{
    public Guid Id { get; set; }
}
