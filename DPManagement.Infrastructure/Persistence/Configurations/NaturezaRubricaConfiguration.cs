using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DPManagement.Domain.Entities;

namespace DPManagement.Infrastructure.Persistence.Configurations
{
    public class NaturezaRubricaConfiguration : IEntityTypeConfiguration<NaturezaRubrica>
    {
        public void Configure(EntityTypeBuilder<NaturezaRubrica> builder)
        {
            builder.ToTable("NaturezaRubricas");

            builder.HasKey(x => x.Codigo);

            builder.Property(x => x.Codigo)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.Nome)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.Descricao)
                .HasMaxLength(2000)
                .IsRequired();

            builder.Property(x => x.Inicio)
                .IsRequired();

            builder.Property(x => x.Termino)
                .IsRequired(false);

            builder.HasData(
                new NaturezaRubrica { Codigo = "1000", Nome = "Salário, vencimento, soldo", Descricao = "Corresponde ao salário básico contratual, inclusive o valor pago ao estagiário e ao bolsista por força da Lei nº 11.788/2008.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "1001", Nome = "Subsídio", Descricao = "Corresponde à remuneração paga na forma de subsídio aos ocupantes de cargos de que trata o art. 39, § 4º, da Constituição Federal de 1988.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "1002", Nome = "Descanso semanal remunerado - DSR", Descricao = "Valor correspondente a um dia de trabalho por semana, ou em feriados, destinado ao repouso dos trabalhadores, desde que não esteja incluído no salário.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc), Termino = new DateTime(2024, 4, 30, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "1003", Nome = "Horas extraordinárias", Descricao = "Valor correspondente à hora extraordinária de trabalho, paga a qualquer título.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "1004", Nome = "Horas extraordinárias - Banco de horas", Descricao = "Valor correspondente a pagamento das horas extraordinárias, inicialmente destinadas para o banco de horas e que não foram compensadas.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "1005", Nome = "Direito de arena", Descricao = "Valores relativos a direito de arena decorrente do espetáculo, devidos ao atleta.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "9232", Nome = "Contribuição sindical - Assistencial", Descricao = "Valor correspondente ao desconto de contribuição assistencial, devida ao sindicato.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "9233", Nome = "Contribuição sindical - Confederativa", Descricao = "Valor correspondente ao desconto de contribuição confederativa, devida ao sindicato.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "9240", Nome = "Alimentação concedida em pecúnia - Desconto", Descricao = "Desconto referente à alimentação concedida em pecúnia.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "9241", Nome = "Alimentação em ticket ou cartão, vinculada ao PAT - Desconto", Descricao = "Desconto referente à participação do trabalhador no custeio da alimentação em ticket ou cartão, vinculada ao PAT.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "9271", Nome = "Indenizações por rescisão de contrato", Descricao = "Valor correspondente ao desconto de indenizações por rescisão de contrato.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
                new NaturezaRubrica { Codigo = "9299", Nome = "Outros descontos", Descricao = "Outros descontos não discriminados nos códigos acima.", Inicio = new DateTime(2014, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
            );
        }
    }
}
