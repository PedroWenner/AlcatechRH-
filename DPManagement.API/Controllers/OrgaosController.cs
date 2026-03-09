using DPManagement.Application.DTOs;
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
    public async Task<IActionResult> GetAll()
    {
        var orgaos = await _service.ObterTodosAsync();
        var dtos = orgaos.Select(o => new OrgaoDto
        {
            Id = o.Id,
            Nome = o.Nome,
            Abreviatura = o.Abreviatura,
            Nivel = o.Nivel,
            OrgaoPaiId = o.OrgaoPaiId,
            NomeAbreviaturaPai = o.OrgaoPai != null ? $"{o.OrgaoPai.Nome} ({o.OrgaoPai.Abreviatura})" : string.Empty,
            Ativo = o.Ativo
        });

        return Ok(dtos);
    }

    [HttpGet("nivel/{nivel}")]
    public async Task<IActionResult> GetByLevel(int nivel)
    {
        var orgaos = await _service.ObterPorNivelAsync(nivel);
        var dtos = orgaos.Select(o => new OrgaoDto
        {
             Id = o.Id,
             Nome = o.Nome,
             Abreviatura = o.Abreviatura,
             Nivel = o.Nivel
        });
        return Ok(dtos);
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

        var created = await _service.AdicionarAsync(entidade);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] OrgaoRequestDto request)
    {
        var orgao = await _service.ObterPorIdAsync(id);
        if (orgao == null) return NotFound();

        // Evitar ciclo infinito: Se a requisição tentar botar o próprio ID como pai
        if (request.OrgaoPaiId == id)
            return BadRequest("Um órgão não pode ser pai dele mesmo.");

        orgao.Nome = request.Nome;
        orgao.Abreviatura = request.Abreviatura;
        orgao.Nivel = request.Nivel;
        orgao.OrgaoPaiId = request.OrgaoPaiId;

        await _service.AtualizarAsync(orgao);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/ativar")]
    public async Task<IActionResult> ToggleActive(Guid id, [FromQuery] bool ativo)
    {
        await _service.AlternarStatusAsync(id, ativo);
        return NoContent();
    }
}
