using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/centro-custos")]
public class CentroCustosController : ControllerBase
{
    private readonly ICentroCustoService _service;
    private readonly IValidator<CentroCustoRequestDto> _validator;

    public CentroCustosController(ICentroCustoService service, IValidator<CentroCustoRequestDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? descricao, [FromQuery] Guid? orgaoId)
    {
        var result = await _service.ObterTodosAsync(descricao, orgaoId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.ObterPorIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CentroCustoRequestDto request)
    {
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var result = await _service.AdicionarAsync(request);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CentroCustoRequestDto request)
    {
        if (id != request.Id) return BadRequest(OperationResult.Failure("ID do centro de custo não confere."));

        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var result = await _service.AtualizarAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ToggleStatus(Guid id, [FromBody] bool ativo)
    {
        var result = await _service.AtivarInativarAsync(id, ativo);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.RemoverAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
