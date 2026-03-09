namespace DPManagement.Application.DTOs;

public class OrgaoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    public int Nivel { get; set; } = 1;

    public Guid? OrgaoPaiId { get; set; }
    public string NomeAbreviaturaPai { get; set; } = string.Empty;
    
    public bool Ativo { get; set; } = true;
}

public class OrgaoRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string Abreviatura { get; set; } = string.Empty;
    public int Nivel { get; set; } = 1;
    public Guid? OrgaoPaiId { get; set; }
}
