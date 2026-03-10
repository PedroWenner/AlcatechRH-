using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
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
        var result = await _perfilService.ListarTodosAsync();
        if (!result.Success) return BadRequest(result);

        var dtos = result.Data.Select(p => new PerfilDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao,
            Ativo = p.Ativo,
            PermissoesIds = p.PerfilPermissoes.Select(pp => pp.PermissaoId).ToList()
        });

        return Ok(OperationResult<IEnumerable<PerfilDto>>.Ok(dtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _perfilService.ObterPorIdAsync(id);
        if (!result.Success) return NotFound(result);

        var perfil = result.Data;
        var dto = new PerfilDto
        {
            Id = perfil.Id,
            Nome = perfil.Nome,
            Descricao = perfil.Descricao,
            Ativo = perfil.Ativo,
            PermissoesIds = perfil.PerfilPermissoes.Select(pp => pp.PermissaoId).ToList()
        };

        return Ok(OperationResult<PerfilDto>.Ok(dto));
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] PerfilRequestDto request)
    {
        var perfil = new Perfil
        {
            Nome = request.Nome,
            Descricao = request.Descricao
        };

        var result = await _perfilService.AdicionarAsync(perfil);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] PerfilRequestDto request)
    {
        var getResult = await _perfilService.ObterPorIdAsync(id);
        if (!getResult.Success) return NotFound(getResult);

        var perfil = getResult.Data;
        perfil.Nome = request.Nome;
        perfil.Descricao = request.Descricao;

        var result = await _perfilService.AtualizarAsync(perfil);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlternarStatus(Guid id, [FromBody] bool ativo)
    {
        var result = await _perfilService.AtivarInativarAsync(id, ativo);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var result = await _perfilService.RemoverAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id}/permissoes")]
    public async Task<IActionResult> AtribuirPermissoes(Guid id, [FromBody] IEnumerable<Guid> permissaoIds)
    {
        var result = await _perfilService.AtribuirPermissoesAsync(id, permissaoIds);
        return result.Success ? Ok(result) : BadRequest(result);
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
