using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Application.Common;
using DPManagement.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace DPManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ColaboradoresController : ControllerBase
{
    private readonly IColaboradorAppService _colaboradorAppService;
    private readonly IViaCepService _viaCepService;
    private readonly IValidator<CreateColaboradorDto> _createValidator;
    private readonly IValidator<UpdateColaboradorDto> _updateValidator;

    public ColaboradoresController(
        IColaboradorAppService colaboradorAppService,
        IViaCepService viaCepService,
        IValidator<CreateColaboradorDto> createValidator,
        IValidator<UpdateColaboradorDto> updateValidator)
    {
        _colaboradorAppService = colaboradorAppService;
        _viaCepService = viaCepService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null,
        [FromQuery] string? cpf = null,
        [FromQuery] Guid? cargoId = null)
    {
        var result = await _colaboradorAppService.GetPagedAsync(page, pageSize, nome, cpf, cargoId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllOld()
    {
        var result = await _colaboradorAppService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _colaboradorAppService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateColaboradorDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var result = await _colaboradorAppService.AddAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateColaboradorDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        if (id != dto.Id) return BadRequest(OperationResult.Failure("ID do colaborador não confere."));
        var result = await _colaboradorAppService.UpdateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _colaboradorAppService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("cep/{cep}")]
    public async Task<IActionResult> GetCep(string cep)
    {
        var result = await _viaCepService.GetEnderecoByCepAsync(cep);
        return result == null ? NotFound() : Ok(result);
    }
}
