using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Application.Common;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

namespace DPManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CargosController : ControllerBase
{
    private readonly ICargoAppService _cargoAppService;
    private readonly IValidator<CreateCargoDto> _createValidator;
    private readonly IValidator<UpdateCargoDto> _updateValidator;

    public CargosController(
        ICargoAppService cargoAppService,
        IValidator<CreateCargoDto> createValidator,
        IValidator<UpdateCargoDto> updateValidator)
    {
        _cargoAppService = cargoAppService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? nome = null,
        [FromQuery] string? cbo = null)
    {
        var result = await _cargoAppService.GetPagedAsync(page, pageSize, nome, cbo);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllOld()
    {
        var result = await _cargoAppService.GetAllAsync();
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _cargoAppService.GetByIdAsync(id);
        if (!result.Success) return NotFound(result);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCargoDto dto)
    {
        var validation = await _createValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        var result = await _cargoAppService.AddAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCargoDto dto)
    {
        var validation = await _updateValidator.ValidateAsync(dto);
        if (!validation.IsValid)
        {
            var errors = validation.Errors.Select(e => e.ErrorMessage).ToList();
            return BadRequest(OperationResult.Failure("Falha na validação.", errors.ToArray()));
        }

        if (id != dto.Id) return BadRequest(OperationResult.Failure("ID do cargo não confere."));
        var result = await _cargoAppService.UpdateAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _cargoAppService.DeleteAsync(id);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
