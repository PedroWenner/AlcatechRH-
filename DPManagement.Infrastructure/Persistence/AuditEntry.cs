using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence;

public class AuditEntry
{
    public AuditEntry(EntityEntry entry)
    {
        Entry = entry;
    }

    public EntityEntry Entry { get; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string TableName { get; set; } = string.Empty;
    public Dictionary<string, object?> KeyValues { get; } = new();
    public Dictionary<string, object?> OldValues { get; } = new();
    public Dictionary<string, object?> NewValues { get; } = new();
    public List<string> ChangedColumns { get; } = new();
    public string Action { get; set; } = string.Empty;

    public AuditLog ToAuditLog()
    {
        var audit = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = UserId,
            UserName = UserName,
            TableName = TableName,
            CreatedAt = DateTime.UtcNow,
            Action = Action,
            EntityId = JsonSerializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            ChangedColumns = ChangedColumns.Count == 0 ? null : JsonSerializer.Serialize(ChangedColumns)
        };
        return audit;
    }
}
