using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/vinculos")]
public class VinculosController : ControllerBase
{
    private readonly IVinculoService _service;

    public VinculosController(IVinculoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? matricula = null, [FromQuery] string? nomeColaborador = null)
    {
        var result = await _service.GetPaginatedAsync(page, pageSize, matricula, nomeColaborador);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUnpaginated([FromQuery] bool showDeleted = false)
    {
        var vinculos = await _service.GetAllAsync(showDeleted);
        return Ok(vinculos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var vinculo = await _service.GetByIdAsync(id);
        if (vinculo == null) return NotFound();
        return Ok(vinculo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VinculoCreateUpdateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] VinculoCreateUpdateDto dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrado")) return NotFound();
            throw;
        }
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        try
        {
            await _service.ToggleStatusAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrado")) return NotFound();
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrado")) return NotFound();
            throw;
        }
    }
}
