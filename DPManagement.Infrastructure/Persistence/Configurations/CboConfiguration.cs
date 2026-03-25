using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class CboConfiguration : IEntityTypeConfiguration<Cbo>
{
    public void Configure(EntityTypeBuilder<Cbo> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Codigo)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(255);
    }
}
