using System.ComponentModel.DataAnnotations;

namespace DPManagement.Application.DTOs;

public class UsuarioResponseDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid PerfilId { get; set; }
    public string PerfilNome { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}

public class CriarUsuarioDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O perfil é obrigatório")]
    public Guid PerfilId { get; set; }
}

public class AtualizarUsuarioDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
    public string Email { get; set; } = string.Empty;

    public string? Senha { get; set; }

    [Required(ErrorMessage = "O perfil é obrigatório")]
    public Guid PerfilId { get; set; }
}
