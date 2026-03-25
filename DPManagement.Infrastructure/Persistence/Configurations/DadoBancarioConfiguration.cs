using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations;

public class DadoBancarioConfiguration : IEntityTypeConfiguration<DadoBancario>
{
    public void Configure(EntityTypeBuilder<DadoBancario> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.CodigoBanco)
            .HasMaxLength(10);

        builder.Property(d => d.Agencia)
            .HasMaxLength(20);

        builder.Property(d => d.DigitoAgencia)
            .HasMaxLength(5);

        builder.Property(d => d.Conta)
            .HasMaxLength(30);

        builder.Property(d => d.DigitoConta)
            .HasMaxLength(5);

        builder.Property(d => d.Operacao)
            .HasMaxLength(10);

        builder.HasOne(d => d.Colaborador)
            .WithMany()
            .HasForeignKey(d => d.ColaboradorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Banco)
            .WithMany()
            .HasForeignKey(d => d.BancoId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
