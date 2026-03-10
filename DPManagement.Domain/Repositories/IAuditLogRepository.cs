using DPManagement.Domain.Entities;

namespace DPManagement.Domain.Repositories;

public interface IAuditLogRepository
{
    Task<(IEnumerable<AuditLog> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null, 
        string? userId = null, 
        string? tableName = null, 
        string? action = null);
}
