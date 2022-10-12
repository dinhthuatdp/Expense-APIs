using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.ExpenseManagement.Api.Migrations
{
    public partial class UpdateEntityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "dbo",
                table: "EntityType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "dbo",
                table: "EntityType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                schema: "dbo",
                table: "EntityType",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                schema: "dbo",
                table: "EntityType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                schema: "dbo",
                table: "EntityType",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "dbo",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "dbo",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                schema: "dbo",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                schema: "dbo",
                table: "EntityType");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                schema: "dbo",
                table: "EntityType");
        }
    }
}
