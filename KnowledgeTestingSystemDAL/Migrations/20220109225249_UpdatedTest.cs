using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KnowledgeTestingSystemDAL.Migrations
{
    public partial class UpdatedTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TimeToEnd",
                table: "Tests",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeToEnd",
                table: "Tests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
