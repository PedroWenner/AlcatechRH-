using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDadosBancarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DadosBancarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CodigoBanco = table.Column<string>(type: "text", nullable: false),
                    Agencia = table.Column<string>(type: "text", nullable: false),
                    DigitoAgencia = table.Column<string>(type: "text", nullable: false),
                    Conta = table.Column<string>(type: "text", nullable: false),
                    DigitoConta = table.Column<string>(type: "text", nullable: false),
                    Operacao = table.Column<string>(type: "text", nullable: false),
                    ColaboradorId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosBancarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DadosBancarios_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DadosBancarios_ColaboradorId",
                table: "DadosBancarios",
                column: "ColaboradorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosBancarios");
        }
    }
}
