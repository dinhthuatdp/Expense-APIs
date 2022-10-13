using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2.ExpenseManagement.Api.Migrations
{
    public partial class UpdateExpenseAddCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CategoryID",
                schema: "dbo",
                table: "Expense",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CategoryID",
                schema: "dbo",
                table: "Expense",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Category_CategoryID",
                schema: "dbo",
                table: "Expense",
                column: "CategoryID",
                principalSchema: "dbo",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Category_CategoryID",
                schema: "dbo",
                table: "Expense");

            migrationBuilder.DropIndex(
                name: "IX_Expense_CategoryID",
                schema: "dbo",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                schema: "dbo",
                table: "Expense");
        }
    }
}
