namespace DPManagement.Application.DTOs;

public class CentroCustoDto
{
    public Guid Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public Guid OrgaoId { get; set; }
    public string OrgaoNome { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}

public class CentroCustoRequestDto
{
    public Guid? Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public Guid OrgaoId { get; set; }
}
