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
            return BadRequest("O termo de busca (term) é obrigatório.");
        }

        return Ok(await _bancoService.SearchAsync(term));
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetByCodigo(string codigo)
    {
        var banco = await _bancoService.GetByCodigoAsync(codigo);
        return banco == null ? NotFound() : Ok(banco);
    }
}
