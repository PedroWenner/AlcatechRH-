namespace DPManagement.Application.DTOs;

public class DadoBancarioDto
{
    public Guid Id { get; set; }
    public Guid ColaboradorId { get; set; }
    public Guid? BancoId { get; set; }
    public string NomeBanco { get; set; } = string.Empty;
    public string CodigoBanco { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string DigitoAgencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
    public string DigitoConta { get; set; } = string.Empty;
    public string Operacao { get; set; } = string.Empty;
}

public class DadoBancarioRequestDto
{
    public Guid ColaboradorId { get; set; }
    public Guid? BancoId { get; set; }
    public string CodigoBanco { get; set; } = string.Empty;
    public string Agencia { get; set; } = string.Empty;
    public string DigitoAgencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
    public string DigitoConta { get; set; } = string.Empty;
    public string Operacao { get; set; } = string.Empty;
}
