using DPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class PerfilPermissaoConfiguration : IEntityTypeConfiguration<PerfilPermissao>
{
    public void Configure(EntityTypeBuilder<PerfilPermissao> builder)
    {
        builder.HasKey(e => new { e.PerfilId, e.PermissaoId });

        builder.HasOne(e => e.Perfil)
            .WithMany(p => p.PerfilPermissoes)
            .HasForeignKey(e => e.PerfilId);

        builder.HasOne(e => e.Permissao)
            .WithMany(p => p.PerfilPermissoes)
            .HasForeignKey(e => e.PermissaoId);

        // Seed Relacionamentos (Admin tem tudo, RH tem acesso à folha mas não calcula)
        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var rhId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        
        var folhaAcessar = Guid.Parse("a1111111-1111-1111-1111-111111111111");
        var folhaCalcular = Guid.Parse("a1111111-1111-1111-1111-111111111112");

        builder.HasData(
            new PerfilPermissao { PerfilId = adminId, PermissaoId = folhaAcessar },
            new PerfilPermissao { PerfilId = adminId, PermissaoId = folhaCalcular },
            new PerfilPermissao { PerfilId = rhId, PermissaoId = folhaAcessar }
            // RH não tem permissão para folhaCalcular propositalmente para o teste
        );
    }
}
