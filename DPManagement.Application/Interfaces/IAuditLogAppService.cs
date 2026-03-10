using DPManagement.Application.Common;
using DPManagement.Application.DTOs;

namespace DPManagement.Application.Interfaces;

public interface IAuditLogAppService
{
    Task<OperationResult<PagedResultDto<AuditLogDto>>> GetPagedAsync(
        int page, 
        int pageSize, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null, 
        string? userId = null, 
        string? tableName = null, 
        string? action = null);
}
