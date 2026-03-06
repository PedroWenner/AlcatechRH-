using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CbosController : ControllerBase
{
    private readonly ICboAppService _cboAppService;

    public CbosController(ICboAppService cboAppService)
    {
        _cboAppService = cboAppService;
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term) || term.Length < 2) 
            return Ok(Enumerable.Empty<object>());

        return Ok(await _cboAppService.SearchAsync(term));
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetByCodigo(string codigo)
    {
        var cbo = await _cboAppService.GetByCodigoAsync(codigo);
        return cbo == null ? NotFound() : Ok(cbo);
    }
}
