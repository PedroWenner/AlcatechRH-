using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/permissoes")]
public class PermissoesController : ControllerBase
{
    private readonly IPermissaoService _permissaoService;

    public PermissoesController(IPermissaoService permissaoService)
    {
        _permissaoService = permissaoService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var permissoes = await _permissaoService.ListarTodasAsync();
        var dtos = permissoes.Select(p => new PermissaoDto
        {
            Id = p.Id,
            Modulo = p.Modulo,
            ModuloPai = p.ModuloPai,
            Acao = p.Acao,
            Descricao = p.Descricao,
            Ativo = p.Ativo
        });

        return Ok(dtos);
    }

    [HttpGet("matriz")]
    public async Task<IActionResult> ObterMatriz()
    {
        var permissoes = await _permissaoService.ListarTodasAsync();
        
        var matriz = permissoes
            .Where(p => p.Ativo && !p.IsDeleted) // Safely ensure we only show active ones
            .GroupBy(p => p.ModuloPai ?? string.Empty) // Group first by Parent Module (fallback to Empty)
            .OrderBy(gp => string.IsNullOrEmpty(gp.Key) ? 1 : 0).ThenBy(gp => gp.Key) // Sort empty parents to bottom
            .Select(gp => new
            {
                ModuloPai = string.IsNullOrEmpty(gp.Key) ? null : gp.Key,
                ModulosFilhos = gp.GroupBy(p => p.Modulo)
                    .OrderBy(gm => gm.Key)
                    .Select(gm => new 
                    {
                        Modulo = gm.Key,
                        Permissoes = gm.Select(p => new PermissaoDto
                        {
                            Id = p.Id,
                            Modulo = p.Modulo,
                            ModuloPai = p.ModuloPai,
                            Acao = p.Acao,
                            Descricao = p.Descricao,
                            Ativo = p.Ativo
                        }).ToList()
                    }).ToList()
            }).ToList();

        return Ok(matriz);
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] PermissaoRequestDto request)
    {
        var permissao = new DPManagement.Domain.Entities.Permissao
        {
            Modulo = request.Modulo,
            ModuloPai = string.IsNullOrWhiteSpace(request.ModuloPai) ? null : request.ModuloPai,
            Acao = request.Acao,
            Descricao = request.Descricao
        };

        await _permissaoService.AdicionarAsync(permissao);
        return CreatedAtAction(nameof(Listar), new { id = permissao.Id }, permissao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] PermissaoRequestDto request)
    {
        var permissao = await _permissaoService.ObterPorIdAsync(id);
        if (permissao == null) return NotFound();

        permissao.Modulo = request.Modulo;
        permissao.ModuloPai = string.IsNullOrWhiteSpace(request.ModuloPai) ? null : request.ModuloPai;
        permissao.Acao = request.Acao;
        permissao.Descricao = request.Descricao;

        await _permissaoService.AtualizarAsync(permissao);
        return NoContent();
    }

    [HttpPut("{id}/ativar")]
    public async Task<IActionResult> AtivarInativar(Guid id, [FromQuery] bool ativo)
    {
        await _permissaoService.AtivarInativarAsync(id, ativo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        await _permissaoService.RemoverAsync(id);
        return NoContent();
    }
}

public class PermissaoDto
{
    public Guid Id { get; set; }
    public string Modulo { get; set; } = string.Empty;
    public string? ModuloPai { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public bool Ativo { get; set; }
}

public class PermissaoRequestDto
{
    public string Modulo { get; set; } = string.Empty;
    public string? ModuloPai { get; set; }
    public string Acao { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
