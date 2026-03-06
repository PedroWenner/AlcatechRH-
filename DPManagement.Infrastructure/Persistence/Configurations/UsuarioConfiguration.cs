using DPManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Nome)
            .IsRequired()
            .HasMaxLength(150);
            
        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(150);
            
        builder.HasIndex(e => e.Email)
            .IsUnique();
            
        builder.Property(e => e.SenhaHash)
            .IsRequired();
            
        builder.Property(e => e.PerfilId)
            .IsRequired();

        builder.HasOne(e => e.Perfil)
            .WithMany(p => p.Usuarios)
            .HasForeignKey(e => e.PerfilId);

        // Seed inicial de Admin
        builder.HasData(new Usuario
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Nome = "Administrador Geral",
            Email = "admin@dpmanagement.com",
            // Hash da senha "Admin@123" gerado com BCrypt (Work Factor 10)
            SenhaHash = "$2a$10$D/j9H67A9/9J6L8.Qh0t8O8jQ.X2xZ0kH0.x0.x0.x0.x0.x0.x0",
            DataCriacao = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            PerfilId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        });
    }
}
