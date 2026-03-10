using DPManagement.Application.Common;
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
            return Ok(OperationResult<IEnumerable<object>>.Ok(Enumerable.Empty<object>()));

        var result = await _cboAppService.SearchAsync(term);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> GetByCodigo(string codigo)
    {
        var result = await _cboAppService.GetByCodigoAsync(codigo);
        return result.Success ? Ok(result) : NotFound(result);
    }
}
