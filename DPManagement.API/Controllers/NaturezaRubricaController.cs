using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;

namespace DPManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/naturezas-rubricas")]
    public class NaturezaRubricaController : ControllerBase
    {
        private readonly INaturezaRubricaService _service;

        public NaturezaRubricaController(INaturezaRubricaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NaturezaRubricaDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(new { success = true, data = result });
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult<NaturezaRubricaDto>> GetByCodigo(string codigo)
        {
            var result = await _service.GetByCodigoAsync(codigo);
            if (result == null) return NotFound(new { success = false, message = "Natureza não encontrada" });
            return Ok(new { success = true, data = result });
        }
    }
}
