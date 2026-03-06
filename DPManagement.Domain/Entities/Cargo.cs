namespace DPManagement.Domain.Entities;

public class Cargo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Nome { get; set; } = string.Empty;
    public string CBO { get; set; } = string.Empty;
    public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
}
