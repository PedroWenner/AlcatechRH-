using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class VinculoConfiguration : IEntityTypeConfiguration<Vinculo>
{
    public void Configure(EntityTypeBuilder<Vinculo> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Matricula)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(v => v.SalarioBase)
            .HasPrecision(18, 2);

        builder.Property(v => v.RegimeJuridicoId)
            .HasConversion<int>();
            
        builder.Property(v => v.FormaIngressoId)
            .HasConversion<int>();

        builder.HasOne(v => v.Colaborador)
            .WithMany(c => c.Vinculos)
            .HasForeignKey(v => v.ColaboradorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Orgao)
            .WithMany(o => o.Vinculos)
            .HasForeignKey(v => v.OrgaoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.Cargo)
            .WithMany(c => c.Vinculos)
            .HasForeignKey(v => v.CargoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(v => v.CentroCusto)
            .WithMany(cc => cc.Vinculos)
            .HasForeignKey(v => v.CentroCustoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
