using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVinculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vinculos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ColaboradorId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrgaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Matricula = table.Column<string>(type: "text", nullable: false),
                    CargoId = table.Column<Guid>(type: "uuid", nullable: false),
                    RegimeJuridicoId = table.Column<int>(type: "integer", nullable: false),
                    FormaIngressoId = table.Column<int>(type: "integer", nullable: false),
                    CentroCustoId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataAdmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vinculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vinculos_Cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vinculos_CentroCustos_CentroCustoId",
                        column: x => x.CentroCustoId,
                        principalTable: "CentroCustos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vinculos_Colaboradores_ColaboradorId",
                        column: x => x.ColaboradorId,
                        principalTable: "Colaboradores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vinculos_Orgaos_OrgaoId",
                        column: x => x.OrgaoId,
                        principalTable: "Orgaos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_CargoId",
                table: "Vinculos",
                column: "CargoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_CentroCustoId",
                table: "Vinculos",
                column: "CentroCustoId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_ColaboradorId",
                table: "Vinculos",
                column: "ColaboradorId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculos_OrgaoId",
                table: "Vinculos",
                column: "OrgaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vinculos");
        }
    }
}
