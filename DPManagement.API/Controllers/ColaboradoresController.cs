using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColaboradoresController : ControllerBase
{
    private readonly IColaboradorAppService _colaboradorAppService;
    private readonly IViaCepService _viaCepService;

    public ColaboradoresController(IColaboradorAppService colaboradorAppService, IViaCepService viaCepService)
    {
        _colaboradorAppService = colaboradorAppService;
        _viaCepService = viaCepService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _colaboradorAppService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var colaborador = await _colaboradorAppService.GetByIdAsync(id);
        return colaborador == null ? NotFound() : Ok(colaborador);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateColaboradorDto dto)
    {
        await _colaboradorAppService.AddAsync(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateColaboradorDto dto)
    {
        if (id != dto.Id) return BadRequest();
        await _colaboradorAppService.UpdateAsync(dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _colaboradorAppService.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("cep/{cep}")]
    public async Task<IActionResult> GetCep(string cep)
    {
        var result = await _viaCepService.GetEnderecoByCepAsync(cep);
        return result == null ? NotFound() : Ok(result);
    }
}
