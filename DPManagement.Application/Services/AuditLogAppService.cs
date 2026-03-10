using DPManagement.Application.DTOs;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Repositories;

namespace DPManagement.Application.Services;

public class AuditLogAppService : IAuditLogAppService
{
    private readonly IAuditLogRepository _auditLogRepository;

    public AuditLogAppService(IAuditLogRepository auditLogRepository)
    {
        _auditLogRepository = auditLogRepository;
    }

    public async Task<PagedResultDto<AuditLogDto>> GetPagedAsync(
        int page, 
        int pageSize, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null, 
        string? userId = null, 
        string? tableName = null, 
        string? action = null)
    {
        var (items, totalCount) = await _auditLogRepository.GetPagedAsync(page, pageSize, dataInicio, dataFim, userId, tableName, action);
        
        var dtos = items.Select(x => new AuditLogDto
        {
            Id = x.Id,
            TableName = x.TableName,
            EntityId = x.EntityId,
            Action = x.Action,
            OldValues = x.OldValues,
            NewValues = x.NewValues,
            ChangedColumns = x.ChangedColumns,
            UserId = x.UserId,
            UserName = x.UserName,
            CreatedAt = x.CreatedAt
        }).ToList();

        return new PagedResultDto<AuditLogDto>(dtos, totalCount, page, pageSize);
    }
}
