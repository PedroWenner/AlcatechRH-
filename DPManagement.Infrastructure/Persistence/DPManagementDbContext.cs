using Microsoft.EntityFrameworkCore;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence;

public class DPManagementDbContext : DbContext
{
    public DPManagementDbContext(DbContextOptions<DPManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automaticamente todas as classes de configuração (IEntityTypeConfiguration) deste Assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DPManagementDbContext).Assembly);
    }
}
