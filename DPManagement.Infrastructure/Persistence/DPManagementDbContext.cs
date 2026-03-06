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
    public DbSet<Cargo> Cargos { get; set; } = null!;
    public DbSet<Colaborador> Colaboradores { get; set; } = null!;
    public DbSet<Cbo> Cbos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DPManagementDbContext).Assembly);
    }
}
