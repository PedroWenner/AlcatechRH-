using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RubricasController : ControllerBase
{
    private readonly IRubricaService _rubricaService;

    public RubricasController(IRubricaService rubricaService)
    {
        _rubricaService = rubricaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? filtro = null)
    {
        var result = await _rubricaService.GetPaginatedAsync(page, pageSize, filtro);
        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] bool showDeleted = false)
    {
        var result = await _rubricaService.GetAllAsync(showDeleted);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _rubricaService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RubricaCreateUpdateDto dto)
    {
        var result = await _rubricaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RubricaCreateUpdateDto dto)
    {
        await _rubricaService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _rubricaService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        await _rubricaService.ToggleStatusAsync(id);
        return NoContent();
    }
}
