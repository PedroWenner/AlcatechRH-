using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueToRubricaCodigo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rubricas_Codigo",
                table: "Rubricas",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Rubricas_Codigo",
                table: "Rubricas");
        }
    }
}
