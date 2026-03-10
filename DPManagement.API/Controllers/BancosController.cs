using DPManagement.Application.Common;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BancosController : ControllerBase
{
    private readonly IBancoService _bancoService;

    public BancosController(IBancoService bancoService)
    {
        _bancoService = bancoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
        {
            return BadRequest(OperationResult.Failure("O termo de busca (term) é obrigatório."));
        }

        var result = await _bancoService.SearchAsync(term);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetByCodigo(string codigo)
    {
        var result = await _bancoService.GetByCodigoAsync(codigo);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
