using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/centro-custos")]
public class CentroCustosController : ControllerBase
{
    private readonly ICentroCustoService _service;

    public CentroCustosController(ICentroCustoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? descricao, [FromQuery] Guid? orgaoId)
    {
        var centros = await _service.ObterTodosAsync(descricao, orgaoId);
        var dtos = centros.Select(c => new CentroCustoDto
        {
            Id = c.Id,
            Descricao = c.Descricao,
            OrgaoId = c.OrgaoId,
            OrgaoNome = c.Orgao != null ? $"{c.Orgao.Nome} ({c.Orgao.Abreviatura})" : string.Empty,
            Ativo = c.Ativo
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var c = await _service.ObterPorIdAsync(id);
        if (c == null) return NotFound();

        return Ok(new CentroCustoDto
        {
            Id = c.Id,
            Descricao = c.Descricao,
            OrgaoId = c.OrgaoId,
            OrgaoNome = c.Orgao != null ? $"{c.Orgao.Nome} ({c.Orgao.Abreviatura})" : string.Empty,
            Ativo = c.Ativo
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CentroCustoRequestDto request)
    {
        var centro = new CentroCusto
        {
            Descricao = request.Descricao,
            OrgaoId = request.OrgaoId
        };

        await _service.AdicionarAsync(centro);
        return CreatedAtAction(nameof(GetById), new { id = centro.Id }, centro);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CentroCustoRequestDto request)
    {
        var centro = await _service.ObterPorIdAsync(id);
        if (centro == null) return NotFound();

        centro.Descricao = request.Descricao;
        centro.OrgaoId = request.OrgaoId;

        await _service.AtualizarAsync(centro);
        return NoContent();
    }

    [HttpPut("{id}/ativar")]
    public async Task<IActionResult> ToggleAtivo(Guid id, [FromQuery] bool ativo)
    {
        await _service.AtivarInativarAsync(id, ativo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
}
