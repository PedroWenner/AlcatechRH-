using Microsoft.EntityFrameworkCore;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence;

public class DPManagementDbContext : DbContext
{
    public DPManagementDbContext(DbContextOptions<DPManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Perfil> Perfis { get; set; } = null!;
    public DbSet<Permissao> Permissoes { get; set; } = null!;
    public DbSet<PerfilPermissao> PerfilPermissoes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplica automaticamente todas as classes de configuração (IEntityTypeConfiguration) deste Assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DPManagementDbContext).Assembly);
    }
}
