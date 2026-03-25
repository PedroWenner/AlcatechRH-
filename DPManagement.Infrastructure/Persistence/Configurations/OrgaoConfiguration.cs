using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class OrgaoConfiguration : IEntityTypeConfiguration<Orgao>
{
    public void Configure(EntityTypeBuilder<Orgao> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Abreviatura)
            .HasMaxLength(50);

        builder.HasOne(o => o.OrgaoPai)
            .WithMany(o => o.SubOrgaos)
            .HasForeignKey(o => o.OrgaoPaiId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
