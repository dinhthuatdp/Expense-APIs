using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.ExpenseManagement.Api.Migrations
{
    public partial class EntityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityType",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityType", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityType_Name_Type",
                schema: "dbo",
                table: "EntityType",
                columns: new[] { "Name", "Type" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [Type] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityType",
                schema: "dbo");
        }
    }
}
