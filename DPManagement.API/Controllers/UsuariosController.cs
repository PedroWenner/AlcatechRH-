using DPManagement.Application.DTOs;
using DPManagement.Application.Services;
using DPManagement.Domain.Entities;
using DPManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly DPManagementDbContext _context;
    private readonly IAuthService _authService;

    public UsuariosController(DPManagementDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpGet]
    public async Task<IActionResult> ListarTodos()
    {
        var usuarios = await _context.Usuarios
            .Include(u => u.Perfil)
            .OrderBy(u => u.Nome)
            .Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                PerfilId = u.PerfilId,
                PerfilNome = u.Perfil != null ? u.Perfil.Nome : "Indefinido",
                DataCriacao = u.DataCriacao
            })
            .ToListAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Perfil)
            .Where(u => u.Id == id)
            .Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                Nome = u.Nome,
                Email = u.Email,
                PerfilId = u.PerfilId,
                PerfilNome = u.Perfil != null ? u.Perfil.Nome : "Indefinido",
                DataCriacao = u.DataCriacao
            })
            .FirstOrDefaultAsync();

        if (usuario == null)
            return NotFound(new { Mensagem = "Usuário não encontrado." });

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioDto dto)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
        {
            return BadRequest(new { Mensagem = "Já existe um usuário cadastrado com este e-mail." });
        }

        var perfil = await _context.Perfis.FindAsync(dto.PerfilId);
        if (perfil == null)
        {
            return BadRequest(new { Mensagem = "Perfil não encontrado." });
        }

        var novoUsuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = _authService.HashSenha(dto.Senha),
            PerfilId = dto.PerfilId,
            DataCriacao = DateTime.UtcNow
        };

        _context.Usuarios.Add(novoUsuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterPorId), new { id = novoUsuario.Id }, new UsuarioResponseDto
        {
            Id = novoUsuario.Id,
            Nome = novoUsuario.Nome,
            Email = novoUsuario.Email,
            PerfilId = novoUsuario.PerfilId,
            PerfilNome = perfil.Nome,
            DataCriacao = novoUsuario.DataCriacao
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioDto dto)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound(new { Mensagem = "Usuário não encontrado." });

        if (usuario.Email != dto.Email && await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
        {
             return BadRequest(new { Mensagem = "Este e-mail já está sendo utilizado por outro usuário." });
        }

        var perfil = await _context.Perfis.FindAsync(dto.PerfilId);
        if (perfil == null)
        {
            return BadRequest(new { Mensagem = "Perfil não encontrado." });
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.PerfilId = dto.PerfilId;

        if (!string.IsNullOrWhiteSpace(dto.Senha))
        {
            usuario.SenhaHash = _authService.HashSenha(dto.Senha);
        }

        _context.Usuarios.Update(usuario);
        await _context.SaveChangesAsync();

        return Ok(new { Mensagem = "Usuário atualizado com sucesso." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
            return NotFound(new { Mensagem = "Usuário não encontrado." });

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
