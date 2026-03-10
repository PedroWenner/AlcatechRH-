using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/perfis")]
public class PerfisController : ControllerBase
{
    private readonly IPerfilService _perfilService;

    public PerfisController(IPerfilService perfilService)
    {
        _perfilService = perfilService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var perfis = await _perfilService.ListarTodosAsync();
        var dtos = perfis.Select(p => new PerfilDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Ativo = p.Ativo,
            PermissoesIds = p.PerfilPermissoes.Select(pp => pp.PermissaoId).ToList()
        });

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PerfilRequestDto request)
    {
        var perfil = new DPManagement.Domain.Entities.Perfil
        {
            Nome = request.Nome,
            Descricao = request.Descricao
        };

        await _perfilService.AdicionarAsync(perfil);
        return CreatedAtAction(nameof(Listar), new { id = perfil.Id }, perfil);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] PerfilRequestDto request)
    {
        var perfil = await _perfilService.ObterPorIdAsync(id);
        if (perfil == null) return NotFound();

        perfil.Nome = request.Nome;
        perfil.Descricao = request.Descricao;

        await _perfilService.AtualizarAsync(perfil);
        return NoContent();
    }

    [HttpPut("{id}/ativar")]
    public async Task<IActionResult> AtivarInativar(Guid id, [FromQuery] bool ativo)
    {
        await _perfilService.AtivarInativarAsync(id, ativo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _perfilService.RemoverAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/permissoes")]
    public async Task<IActionResult> AtribuirPermissoes(Guid id, [FromBody] List<Guid> permissaoIds)
    {
        await _perfilService.AtribuirPermissoesAsync(id, permissaoIds);
        return NoContent();
    }
}

public class PerfilDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
    public List<Guid> PermissoesIds { get; set; } = new List<Guid>();
}

public class PerfilRequestDto
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
