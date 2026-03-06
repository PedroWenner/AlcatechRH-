using DPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class PermissaoConfiguration : IEntityTypeConfiguration<Permissao>
{
    public void Configure(EntityTypeBuilder<Permissao> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Modulo)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(e => e.Acao)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(e => new { e.Modulo, e.Acao })
            .IsUnique();

        // Seed Permissoes
        builder.HasData(
            // Folha
            new Permissao { Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"), Modulo = "Folha", Acao = "Acessar", Descricao = "Visualizar módulo de folha" },
            new Permissao { Id = Guid.Parse("a1111111-1111-1111-1111-111111111112"), Modulo = "Folha", Acao = "Calcular", Descricao = "Realizar cálculo da folha" },
            // Funcionarios
            new Permissao { Id = Guid.Parse("b2222222-2222-2222-2222-222222222221"), Modulo = "Funcionario", Acao = "Listar", Descricao = "Ver lista de funcionários" },
            new Permissao { Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"), Modulo = "Funcionario", Acao = "Editar", Descricao = "Editar dados de funcionários" }
        );
    }
}
