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

        // eSocial S-1010
        builder.Property(r => r.NatRubr)
            .HasMaxLength(4);

        builder.Property(r => r.IdeTabRubr)
            .HasMaxLength(20);

        builder.Property(r => r.CodIncCP)
            .HasMaxLength(2);

        builder.Property(r => r.CodIncIRRF)
            .HasMaxLength(2);

        builder.Property(r => r.CodIncFGTS)
            .HasMaxLength(2);

        builder.Property(r => r.CodIncPisPasep)
            .HasMaxLength(2);

        builder.Property(r => r.IniValid)
            .IsRequired()
            .HasMaxLength(7); // AAAA-MM

        builder.Property(r => r.FimValid)
            .HasMaxLength(7);
    }
}
