using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LinkBancoToDadoBancario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BancoId",
                table: "DadosBancarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DadosBancarios_BancoId",
                table: "DadosBancarios",
                column: "BancoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DadosBancarios_Bancos_BancoId",
                table: "DadosBancarios",
                column: "BancoId",
                principalTable: "Bancos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DadosBancarios_Bancos_BancoId",
                table: "DadosBancarios");

            migrationBuilder.DropIndex(
                name: "IX_DadosBancarios_BancoId",
                table: "DadosBancarios");

            migrationBuilder.DropColumn(
                name: "BancoId",
                table: "DadosBancarios");
        }
    }
}
