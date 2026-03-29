namespace DPManagement.Application.DTOs
{
    public class PerfilRequestDto
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    public class PerfilDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public List<Guid> PermissoesIds { get; set; } = new List<Guid>();
    }
}
