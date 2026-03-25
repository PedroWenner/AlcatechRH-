using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestMappingFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Metadata only update as the database already contains the required changes/columns
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Metadata only update
        }
    }
}
