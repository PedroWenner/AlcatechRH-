using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class BancoConfiguration : IEntityTypeConfiguration<Banco>
{
    public void Configure(EntityTypeBuilder<Banco> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.CodigoBanco)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(b => b.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.NomeCurto)
            .HasMaxLength(50);
    }
}
