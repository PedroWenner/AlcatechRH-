using DPManagement.Application.Common;
using DPManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DPManagement.API.Controllers;

[Authorize]
[ApiController]
[Route("api/audit-logs")]
public class AuditLogsController : ControllerBase
{
    private readonly IAuditLogAppService _auditLogAppService;

    public AuditLogsController(IAuditLogAppService auditLogAppService)
    {
        _auditLogAppService = auditLogAppService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? dataInicio = null,
        [FromQuery] DateTime? dataFim = null,
        [FromQuery] string? userId = null,
        [FromQuery] string? tableName = null,
        [FromQuery] string? action = null)
    {
        var result = await _auditLogAppService.GetPagedAsync(page, pageSize, dataInicio, dataFim, userId, tableName, action);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
