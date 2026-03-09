using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModuloPaiToPermissoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ModuloPai",
                table: "Permissoes",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111112"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111113"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111114"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222221"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222223"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222224"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333331"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333332"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333334"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444441"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444442"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444443"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444444"),
                column: "ModuloPai",
                value: null);

            migrationBuilder.UpdateData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c5555555-5555-5555-5555-555555555551"),
                column: "ModuloPai",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuloPai",
                table: "Permissoes");
        }
    }
}
