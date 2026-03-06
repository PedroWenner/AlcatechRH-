using Microsoft.EntityFrameworkCore;
using DPManagement.Domain.Entities;
using DPManagement.Application.Interfaces;
using DPManagement.Domain.Interfaces;
using System.Linq.Expressions;

namespace DPManagement.Infrastructure.Persistence;

public class DPManagementDbContext : DbContext
{
    private readonly IUserContext _userContext;

    public DPManagementDbContext(DbContextOptions<DPManagementDbContext> options, IUserContext userContext) : base(options)
    {
        _userContext = userContext;
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Perfil> Perfis { get; set; } = null!;
    public DbSet<Permissao> Permissoes { get; set; } = null!;
    public DbSet<PerfilPermissao> PerfilPermissoes { get; set; } = null!;
    public DbSet<Cargo> Cargos { get; set; } = null!;
    public DbSet<Colaborador> Colaboradores { get; set; } = null!;
    public DbSet<Cbo> Cbos { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaveChanges();
        var auditEntries = HandleAuditing();
        
        var result = await base.SaveChangesAsync(cancellationToken);
        
        if (auditEntries.Any())
        {
            await OnAfterSaveChanges(auditEntries);
            await base.SaveChangesAsync(cancellationToken);
        }

        return result;
    }

    private void OnBeforeSaveChanges()
    {
        ChangeTracker.DetectChanges();

        foreach (var entry in ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
            }
        }
    }

    private List<AuditEntry> HandleAuditing()
    {
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                UserId = _userContext.UserId ?? "System",
                UserName = _userContext.UserName ?? "System"
            };

            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.Action = "Insert";
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.Action = "Delete";
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.Action = "Update";
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }

        return auditEntries;
    }

    private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.Entry.Properties.Where(p => p.Metadata.IsPrimaryKey()))
            {
                auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
            }
            AuditLogs.Add(auditEntry.ToAuditLog());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DPManagementDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(ISoftDelete.IsDeleted)),
                    Expression.Constant(false));
                
                var filter = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
}
