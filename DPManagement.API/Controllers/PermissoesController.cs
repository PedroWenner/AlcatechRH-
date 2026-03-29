using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Application.Validators;
using DPManagement.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/permissoes")]
public class PermissoesController : ControllerBase
{
    private readonly IPermissaoService _service;
    private readonly IValidator<PermissaoRequestDto> _validator;

    public PermissoesController(IPermissaoService service, IValidator<PermissaoRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var result = await _service.ListarTodasAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var result = await _service.ObterPorIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpGet("matriz")]
    public async Task<IActionResult> GetMatriz()
    {
        var result = await _service.ListarTodasAsync();
        if (!result.Success) return BadRequest(result);

        var permissoes = result.Data;
        if (permissoes == null) return Ok(OperationResult<IEnumerable<object>>.Ok(new List<object>()));

        var matriz = permissoes
            .Where(p => p.Ativo && !p.IsDeleted)
            .GroupBy(p => p.ModuloPai ?? string.Empty)
            .OrderBy(gp => string.IsNullOrEmpty(gp.Key) ? 1 : 0).ThenBy(gp => gp.Key)
            .Select(gp => new
            {
                ModuloPai = string.IsNullOrEmpty(gp.Key) ? null : gp.Key,
                ModulosFilhos = gp.GroupBy(p => p.Modulo)
                    .OrderBy(gm => gm.Key)
                    .Select(gm => new 
                    {
                        Modulo = gm.Key,
                        Permissoes = gm.Select(p => new
                        {
                            p.Id,
                            p.Modulo,
                            p.ModuloPai,
                            p.Acao,
                            p.Descricao,
                            p.Ativo
                        }).ToList()
                    }).ToList()
            }).ToList();

        return Ok(OperationResult<object>.Ok(matriz));
    }

    [HttpPost]
    public async Task<IActionResult> Adicionar([FromBody] PermissaoRequestDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var permissao = new Permissao
        {
            Modulo = request.Modulo,
            ModuloPai = request.ModuloPai,
            Acao = request.Acao,
            Descricao = request.Descricao
        };

        var result = await _service.AdicionarAsync(permissao);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(ObterPorId), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] PermissaoRequestDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var getResult = await _service.ObterPorIdAsync(id);
        if (!getResult.Success) return NotFound(OperationResult.Failure("Permissão não encontrada."));

        var permissao = getResult.Data;
        permissao.Modulo = request.Modulo;
        permissao.ModuloPai = request.ModuloPai;
        permissao.Acao = request.Acao;
        permissao.Descricao = request.Descricao;

        var result = await _service.AtualizarAsync(permissao);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> AlternarStatus(Guid id, [FromBody] bool ativo)
    {
        var result = await _service.AtivarInativarAsync(id, ativo);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var result = await _service.RemoverAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}