using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CargosController : ControllerBase
{
    private readonly ICargoAppService _cargoAppService;

    public CargosController(ICargoAppService cargoAppService)
    {
        _cargoAppService = cargoAppService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null,
        [FromQuery] string? cbo = null) 
        => Ok(await _cargoAppService.GetPagedAsync(page, pageSize, nome, cbo));

    [HttpGet("all")]
    public async Task<IActionResult> GetAllOld() => Ok(await _cargoAppService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var cargo = await _cargoAppService.GetByIdAsync(id);
        return cargo == null ? NotFound() : Ok(cargo);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCargoDto dto)
    {
        await _cargoAppService.AddAsync(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateCargoDto dto)
    {
        if (id != dto.Id) return BadRequest();
        await _cargoAppService.UpdateAsync(dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _cargoAppService.DeleteAsync(id);
        return Ok();
    }
}
