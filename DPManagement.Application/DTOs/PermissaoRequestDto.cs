namespace DPManagement.Application.DTOs
{
    public class PermissaoRequestDto
    {
        public string Modulo { get; set; } = string.Empty;
        public string? ModuloPai { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class PermissaoDto
{
    public Guid Id { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string? ModuloPai { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
}
}
