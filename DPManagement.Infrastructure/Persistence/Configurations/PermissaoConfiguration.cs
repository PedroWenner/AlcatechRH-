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

        builder.HasQueryFilter(e => !e.IsDeleted);

        // Seed Permissoes (CRUD por Módulo)
        builder.HasData(
            // Cargos
            new Permissao { Id = Guid.Parse("c1111111-1111-1111-1111-111111111111"), Modulo = "Cargos", Acao = "Visualizar", Descricao = "Listar e ver detalhes de cargos" },
            new Permissao { Id = Guid.Parse("c1111111-1111-1111-1111-111111111112"), Modulo = "Cargos", Acao = "Criar", Descricao = "Criar novos cargos" },
            new Permissao { Id = Guid.Parse("c1111111-1111-1111-1111-111111111113"), Modulo = "Cargos", Acao = "Editar", Descricao = "Editar cargos existentes" },
            new Permissao { Id = Guid.Parse("c1111111-1111-1111-1111-111111111114"), Modulo = "Cargos", Acao = "Excluir", Descricao = "Excluir cargos" },
            
            // Colaboradores
            new Permissao { Id = Guid.Parse("c2222222-2222-2222-2222-222222222221"), Modulo = "Colaboradores", Acao = "Visualizar", Descricao = "Listar e ver detalhes de colaboradores" },
            new Permissao { Id = Guid.Parse("c2222222-2222-2222-2222-222222222222"), Modulo = "Colaboradores", Acao = "Criar", Descricao = "Criar novos colaboradores" },
            new Permissao { Id = Guid.Parse("c2222222-2222-2222-2222-222222222223"), Modulo = "Colaboradores", Acao = "Editar", Descricao = "Editar colaboradores existentes" },
            new Permissao { Id = Guid.Parse("c2222222-2222-2222-2222-222222222224"), Modulo = "Colaboradores", Acao = "Excluir", Descricao = "Excluir colaboradores" },
            
            // Folha
            new Permissao { Id = Guid.Parse("c3333333-3333-3333-3333-333333333331"), Modulo = "Folha", Acao = "Visualizar", Descricao = "Visualizar fechamentos de folha" },
            new Permissao { Id = Guid.Parse("c3333333-3333-3333-3333-333333333332"), Modulo = "Folha", Acao = "Criar", Descricao = "Gerar nova folha" },
            new Permissao { Id = Guid.Parse("c3333333-3333-3333-3333-333333333333"), Modulo = "Folha", Acao = "Editar", Descricao = "Ajustar valores da folha" },
            new Permissao { Id = Guid.Parse("c3333333-3333-3333-3333-333333333334"), Modulo = "Folha", Acao = "Excluir", Descricao = "Excluir ou cancelar folha" },
            
            // Perfis e Permissões
            new Permissao { Id = Guid.Parse("c4444444-4444-4444-4444-444444444441"), Modulo = "Perfis", Acao = "Visualizar", Descricao = "Listar e ver detalhes de perfis" },
            new Permissao { Id = Guid.Parse("c4444444-4444-4444-4444-444444444442"), Modulo = "Perfis", Acao = "Criar", Descricao = "Criar novos perfis" },
            new Permissao { Id = Guid.Parse("c4444444-4444-4444-4444-444444444443"), Modulo = "Perfis", Acao = "Editar", Descricao = "Editar perfis e acessos" },
            new Permissao { Id = Guid.Parse("c4444444-4444-4444-4444-444444444444"), Modulo = "Perfis", Acao = "Excluir", Descricao = "Excluir perfis" },
            
            // Auditoria
            new Permissao { Id = Guid.Parse("c5555555-5555-5555-5555-555555555551"), Modulo = "Auditoria", Acao = "Visualizar", Descricao = "Visualizar logs de auditoria" }
        );
    }
}
