using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class CentroCustoConfiguration : IEntityTypeConfiguration<CentroCusto>
{
    public void Configure(EntityTypeBuilder<CentroCusto> builder)
    {
        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Descricao)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(cc => cc.Orgao)
            .WithMany(o => o.CentrosCustos)
            .HasForeignKey(cc => cc.OrgaoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
