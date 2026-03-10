using DPManagement.Application.DTOs;
using DPManagement.Application.Common;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrgaosController : ControllerBase
{
    private readonly IOrgaoService _service;

    public OrgaosController(IOrgaoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? nome, [FromQuery] string? abreviatura)
    {
        var result = await _service.ObterTodosAsync(nome, abreviatura);
        if (!result.Success) return BadRequest(result);

        var dtos = result.Data.Select(o => new OrgaoDto
        {
            Id = o.Id,
            Nome = o.Nome,
            Abreviatura = o.Abreviatura,
            Nivel = o.Nivel,
            OrgaoPaiId = o.OrgaoPaiId,
            NomeAbreviaturaPai = o.OrgaoPai != null ? $"{o.OrgaoPai.Nome} ({o.OrgaoPai.Abreviatura})" : string.Empty,
            Ativo = o.Ativo
        });

        return Ok(OperationResult<IEnumerable<OrgaoDto>>.Ok(dtos));
    }

    [HttpGet("nivel/{nivel}")]
    public async Task<IActionResult> GetByLevel(int nivel)
    {
        var result = await _service.ObterPorNivelAsync(nivel);
        if (!result.Success) return BadRequest(result);

        var dtos = result.Data.Select(o => new OrgaoDto
        {
             Id = o.Id,
             Nome = o.Nome,
             Abreviatura = o.Abreviatura,
             Nivel = o.Nivel
        });
        return Ok(OperationResult<IEnumerable<OrgaoDto>>.Ok(dtos));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.ObterPorIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrgaoRequestDto request)
    {
        var entidade = new Orgao
        {
            Nome = request.Nome,
            Abreviatura = request.Abreviatura,
            Nivel = request.Nivel,
            OrgaoPaiId = request.OrgaoPaiId
        };

        var result = await _service.AdicionarAsync(entidade);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] OrgaoRequestDto request)
    {
        var orgaoResult = await _service.ObterPorIdAsync(id);
        if (!orgaoResult.Success) return NotFound(orgaoResult);

        var orgao = orgaoResult.Data;

        // Evitar ciclo infinito
        if (request.OrgaoPaiId == id)
            return BadRequest(OperationResult.Failure("Um órgão não pode ser pai dele mesmo."));

        orgao.Nome = request.Nome;
        orgao.Abreviatura = request.Abreviatura;
        orgao.Nivel = request.Nivel;
        orgao.OrgaoPaiId = request.OrgaoPaiId;

        var result = await _service.AtualizarAsync(orgao);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.RemoverAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ToggleStatus(Guid id, [FromBody] bool ativo)
    {
        var result = await _service.AlternarStatusAsync(id, ativo);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
