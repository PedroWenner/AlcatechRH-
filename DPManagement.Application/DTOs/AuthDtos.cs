using DPManagement.Domain.Entities;

namespace DPManagement.Application.DTOs;

public class LoginDto
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class RegistroUsuarioDto
{
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public Guid PerfilId { get; set; }
}

public class AuthRespostaDto
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDto Usuario { get; set; } = null!;
}

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
}
