using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[ApiController]
[Route("api/perfis")]
public class PerfisController : ControllerBase
{
    private readonly IPerfilService _perfilService;

    public PerfisController(IPerfilService perfilService)
    {
        _perfilService = perfilService;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var perfis = await _perfilService.ListarTodosAsync();
        var dtos = perfis.Select(p => new PerfilDto
        {
            Id = p.Id,
            Nome = p.Nome,
            Descricao = p.Descricao
        });

        return Ok(dtos);
    }
}

public class PerfilDto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
