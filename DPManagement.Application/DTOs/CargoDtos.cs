namespace DPManagement.Application.DTOs;

public class CargoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CBO { get; set; } = string.Empty;
}

public class CreateCargoDto
{
    public string Nome { get; set; } = string.Empty;
    public string CBO { get; set; } = string.Empty;
}

public class UpdateCargoDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CBO { get; set; } = string.Empty;
}
