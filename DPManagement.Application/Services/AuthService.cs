using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using DPManagement.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DPManagement.Application.Services;

public interface IAuthService
{
    string HashSenha(string senha);
    bool VerificarSenha(string senha, string hash);
    string GerarTokenJwt(Usuario usuario);
}

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string HashSenha(string senha)
    {
        return BCrypt.Net.BCrypt.HashPassword(senha);
    }

    public bool VerificarSenha(string senha, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(senha, hash);
    }

    public string GerarTokenJwt(Usuario usuario)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Role, usuario.Perfil?.Nome ?? "Funcionario")
        };

        // Adicionar permissões como claims
        if (usuario.Perfil?.PerfilPermissoes != null)
        {
            foreach (var pp in usuario.Perfil.PerfilPermissoes)
            {
                claims.Add(new Claim("Permission", $"{pp.Permissao.Modulo}:{pp.Permissao.Acao}"));
            }
        }

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
