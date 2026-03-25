using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class RubricaConfiguration : IEntityTypeConfiguration<Rubrica>
{
    public void Configure(EntityTypeBuilder<Rubrica> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Codigo)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.Descricao)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Tipo)
            .HasConversion<int>();

        builder.Property(r => r.Rotina)
            .HasConversion<int>();

        builder.HasIndex(r => r.Codigo)
            .IsUnique();
    }
}
