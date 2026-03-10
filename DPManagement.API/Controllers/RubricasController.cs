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
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] bool showDeleted = false)
    {
        var result = await _rubricaService.GetAllAsync(showDeleted);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _rubricaService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RubricaCreateUpdateDto dto)
    {
        var result = await _rubricaService.CreateAsync(dto);
        if (!result.Success) return BadRequest(result);
        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RubricaCreateUpdateDto dto)
    {
        var result = await _rubricaService.UpdateAsync(id, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _rubricaService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _rubricaService.ToggleStatusAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
