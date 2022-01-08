using Microsoft.EntityFrameworkCore.Migrations;

namespace KnowledgeTestingSystemDAL.Migrations
{
    public partial class Updated_Question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfOptions",
                table: "Questions",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfOptions",
                table: "Questions");
        }
    }
}
