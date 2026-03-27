using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddESocialFieldsToRubrica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodIncCP",
                table: "Rubricas",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodIncFGTS",
                table: "Rubricas",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodIncIRRF",
                table: "Rubricas",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodIncPisPasep",
                table: "Rubricas",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FimValid",
                table: "Rubricas",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdeTabRubr",
                table: "Rubricas",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IniValid",
                table: "Rubricas",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NatRubr",
                table: "Rubricas",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodIncCP",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "CodIncFGTS",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "CodIncIRRF",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "CodIncPisPasep",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "FimValid",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "IdeTabRubr",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "IniValid",
                table: "Rubricas");

            migrationBuilder.DropColumn(
                name: "NatRubr",
                table: "Rubricas");
        }
    }
}
