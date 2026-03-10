using DPManagement.Domain.Enums;
using DPManagement.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/catalogos")]
public class CatalogosController : ControllerBase
{
    [HttpGet("regimes-juridicos")]
    public IActionResult GetRegimesJuridicos()
    {
        var regimes = Enum.GetValues(typeof(RegimeJuridico))
            .Cast<RegimeJuridico>()
            .Select(e => new
            {
                Id = (int)e,
                Nome = e.GetDescription()
            });

        return Ok(regimes);
    }

    [HttpGet("formas-ingresso")]
    public IActionResult GetFormasIngresso()
    {
        var formas = Enum.GetValues(typeof(FormaIngresso))
            .Cast<FormaIngresso>()
            .Select(e => new
            {
                Id = (int)e,
                Nome = e.GetDescription()
            });

        return Ok(formas);
    }
}
