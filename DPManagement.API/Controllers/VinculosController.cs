using DPManagement.Application.Common;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/vinculos")]
public class VinculosController : ControllerBase
{
    private readonly IVinculoService _service;
    private readonly IValidator<VinculoCreateUpdateDto> _validator;

    public VinculosController(IVinculoService service, IValidator<VinculoCreateUpdateDto> validator)
    {
        _service = service;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? matricula = null, [FromQuery] string? nomeColaborador = null)
    {
        var result = await _service.GetPaginatedAsync(page, pageSize, matricula, nomeColaborador);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUnpaginated([FromQuery] bool showDeleted = false)
    {
        var result = await _service.GetAllAsync(showDeleted);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VinculoCreateUpdateDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var result = await _service.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] VinculoCreateUpdateDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var result = await _service.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _service.ToggleStatusAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
