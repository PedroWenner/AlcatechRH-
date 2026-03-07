using DPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class PerfilConfiguration : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.HasIndex(e => e.Nome)
            .IsUnique();

        builder.Property(e => e.Descricao)
            .HasMaxLength(250);

        builder.HasQueryFilter(e => !e.IsDeleted);

        // Seed Perfis
        builder.HasData(
            new Perfil { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Nome = "Admin", Descricao = "Acesso total ao sistema" },
            new Perfil { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Nome = "RH", Descricao = "Gestão de pessoas e folha" },
            new Perfil { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Nome = "Funcionario", Descricao = "Acesso básico" }
        );
    }
}
