using DPManagement.Domain.Entities;
using DPManagement.Domain.Repositories;
using DPManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DPManagement.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly DPManagementDbContext _context;

    public AuditLogRepository(DPManagementDbContext context)
    {
        _context = context;
    }

    public async Task<(IEnumerable<AuditLog> Items, int TotalCount)> GetPagedAsync(
        int page, 
        int pageSize, 
        DateTime? dataInicio = null, 
        DateTime? dataFim = null, 
        string? userId = null, 
        string? tableName = null, 
        string? action = null)
    {
        var query = _context.AuditLogs.AsNoTracking();

        if (dataInicio.HasValue)
        {
            var inicio = DateTime.SpecifyKind(dataInicio.Value.Date, DateTimeKind.Utc);
            query = query.Where(x => x.CreatedAt.Date >= inicio);
        }

        if (dataFim.HasValue)
        {
            var fim = DateTime.SpecifyKind(dataFim.Value.Date, DateTimeKind.Utc);
            query = query.Where(x => x.CreatedAt.Date <= fim);
        }

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(x => x.UserId == userId);

        if (!string.IsNullOrEmpty(tableName))
            query = query.Where(x => x.TableName == tableName);

        if (!string.IsNullOrEmpty(action))
            query = query.Where(x => x.Action == action);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
