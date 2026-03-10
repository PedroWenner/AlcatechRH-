using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePermissoesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Acao", "Ativo", "Descricao", "IsDeleted", "Modulo" },
                values: new object[,]
                {
                    { new Guid("c1111111-1111-1111-1111-111111111111"), "Visualizar", true, "Listar e ver detalhes de cargos", false, "Cargos" },
                    { new Guid("c1111111-1111-1111-1111-111111111112"), "Criar", true, "Criar novos cargos", false, "Cargos" },
                    { new Guid("c1111111-1111-1111-1111-111111111113"), "Editar", true, "Editar cargos existentes", false, "Cargos" },
                    { new Guid("c1111111-1111-1111-1111-111111111114"), "Excluir", true, "Excluir cargos", false, "Cargos" },
                    { new Guid("c2222222-2222-2222-2222-222222222221"), "Visualizar", true, "Listar e ver detalhes de colaboradores", false, "Colaboradores" },
                    { new Guid("c2222222-2222-2222-2222-222222222222"), "Criar", true, "Criar novos colaboradores", false, "Colaboradores" },
                    { new Guid("c2222222-2222-2222-2222-222222222223"), "Editar", true, "Editar colaboradores existentes", false, "Colaboradores" },
                    { new Guid("c2222222-2222-2222-2222-222222222224"), "Excluir", true, "Excluir colaboradores", false, "Colaboradores" },
                    { new Guid("c3333333-3333-3333-3333-333333333331"), "Visualizar", true, "Visualizar fechamentos de folha", false, "Folha" },
                    { new Guid("c3333333-3333-3333-3333-333333333332"), "Criar", true, "Gerar nova folha", false, "Folha" },
                    { new Guid("c3333333-3333-3333-3333-333333333333"), "Editar", true, "Ajustar valores da folha", false, "Folha" },
                    { new Guid("c3333333-3333-3333-3333-333333333334"), "Excluir", true, "Excluir ou cancelar folha", false, "Folha" },
                    { new Guid("c4444444-4444-4444-4444-444444444441"), "Visualizar", true, "Listar e ver detalhes de perfis", false, "Perfis" },
                    { new Guid("c4444444-4444-4444-4444-444444444442"), "Criar", true, "Criar novos perfis", false, "Perfis" },
                    { new Guid("c4444444-4444-4444-4444-444444444443"), "Editar", true, "Editar perfis e acessos", false, "Perfis" },
                    { new Guid("c4444444-4444-4444-4444-444444444444"), "Excluir", true, "Excluir perfis", false, "Perfis" },
                    { new Guid("c5555555-5555-5555-5555-555555555551"), "Visualizar", true, "Visualizar logs de auditoria", false, "Auditoria" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111112"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111113"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c1111111-1111-1111-1111-111111111114"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222221"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222223"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c2222222-2222-2222-2222-222222222224"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333331"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333332"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333334"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444441"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444442"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444443"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c4444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Permissoes",
                keyColumn: "Id",
                keyValue: new Guid("c5555555-5555-5555-5555-555555555551"));

            migrationBuilder.InsertData(
                table: "Permissoes",
                columns: new[] { "Id", "Acao", "Ativo", "Descricao", "IsDeleted", "Modulo" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "Acessar", true, "Visualizar módulo de folha", false, "Folha" },
                    { new Guid("a1111111-1111-1111-1111-111111111112"), "Calcular", true, "Realizar cálculo da folha", false, "Folha" },
                    { new Guid("b2222222-2222-2222-2222-222222222221"), "Listar", true, "Ver lista de funcionários", false, "Funcionario" },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "Editar", true, "Editar dados de funcionários", false, "Funcionario" }
                });
        }
    }
}
