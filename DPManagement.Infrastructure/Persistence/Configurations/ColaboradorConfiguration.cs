using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class ColaboradorConfiguration : IEntityTypeConfiguration<Colaborador>
{
    public void Configure(EntityTypeBuilder<Colaborador> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nome).IsRequired().HasMaxLength(200);
        builder.Property(c => c.CPF).IsRequired().HasMaxLength(11); // Stored without masks
        builder.Property(c => c.RG).HasMaxLength(20);
        builder.Property(c => c.PIS).HasMaxLength(11);
        builder.Property(c => c.Telefone).HasMaxLength(15);
        builder.Property(c => c.Celular).HasMaxLength(15);
        builder.Property(c => c.CEP).IsRequired().HasMaxLength(8);
        builder.Property(c => c.Logradouro).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Numero).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Complemento).HasMaxLength(100);
        builder.Property(c => c.Bairro).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Cidade).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Estado).IsRequired().HasMaxLength(2);

        builder.HasOne(c => c.Cargo)
               .WithMany(ca => ca.Colaboradores)
               .HasForeignKey(c => c.CargoId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
