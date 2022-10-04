using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.ExpenseManagement.Api.Migrations
{
    public partial class UpdateSchemaDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "ExpenseDb",
                newName: "Category",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ExpenseDb");

            migrationBuilder.RenameTable(
                name: "Category",
                schema: "dbo",
                newName: "Category",
                newSchema: "ExpenseDb");
        }
    }
}
