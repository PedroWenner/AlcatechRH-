using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNaturezaRubricaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaturezaRubricas",
                columns: table => new
                {
                    Codigo = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Nome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Termino = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturezaRubricas", x => x.Codigo);
                });

            migrationBuilder.InsertData(
                table: "NaturezaRubricas",
                columns: new[] { "Codigo", "Descricao", "Inicio", "Nome", "Termino" },
                values: new object[,]
                {
                    { "1000", "Corresponde ao salário básico contratual, inclusive o valor pago ao estagiário e ao bolsista por força da Lei nº 11.788/2008.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Salário, vencimento, soldo", null },
                    { "1001", "Corresponde à remuneração paga na forma de subsídio aos ocupantes de cargos de que trata o art. 39, § 4º, da Constituição Federal de 1988.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Subsídio", null },
                    { "1002", "Valor correspondente a um dia de trabalho por semana, ou em feriados, destinado ao repouso dos trabalhadores, desde que não esteja incluído no salário.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Descanso semanal remunerado - DSR", new DateTime(2024, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "1003", "Valor correspondente à hora extraordinária de trabalho, paga a qualquer título.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Horas extraordinárias", null },
                    { "1004", "Valor correspondente a pagamento das horas extraordinárias, inicialmente destinadas para o banco de horas e que não foram compensadas.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Horas extraordinárias - Banco de horas", null },
                    { "1005", "Valores relativos a direito de arena decorrente do espetáculo, devidos ao atleta.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Direito de arena", null },
                    { "9232", "Valor correspondente ao desconto de contribuição assistencial, devida ao sindicato.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Contribuição sindical - Assistencial", null },
                    { "9233", "Valor correspondente ao desconto de contribuição confederativa, devida ao sindicato.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Contribuição sindical - Confederativa", null },
                    { "9240", "Desconto referente à alimentação concedida em pecúnia.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Alimentação concedida em pecúnia - Desconto", null },
                    { "9241", "Desconto referente à participação do trabalhador no custeio da alimentação em ticket ou cartão, vinculada ao PAT.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Alimentação em ticket ou cartão, vinculada ao PAT - Desconto", null },
                    { "9271", "Valor correspondente ao desconto de indenizações por rescisão de contrato.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Indenizações por rescisão de contrato", null },
                    { "9299", "Outros descontos não discriminados nos códigos acima.", new DateTime(2014, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Outros descontos", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NaturezaRubricas");
        }
    }
}
