using Microsoft.EntityFrameworkCore.Migrations;

namespace KnowledgeTestingSystemDAL.Migrations
{
    public partial class DatabaseV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionsOptions");

            migrationBuilder.DropTable(
                name: "TestsQuestions");

            migrationBuilder.AddColumn<int>(
                name: "CorrectOptionId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "Questions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Options",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_TestId",
                table: "Questions",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_QuestionId",
                table: "Options",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Tests_TestId",
                table: "Questions",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Options_Questions_QuestionId",
                table: "Options");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Tests_TestId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_TestId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Options_QuestionId",
                table: "Options");

            migrationBuilder.DropColumn(
                name: "CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Options");

            migrationBuilder.CreateTable(
                name: "QuestionsOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrectOptionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionsOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionsOptions_Options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionsOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestsQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestsQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestsQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestsQuestions_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsOptions_OptionId",
                table: "QuestionsOptions",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionsOptions_QuestionId",
                table: "QuestionsOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestsQuestions_QuestionId",
                table: "TestsQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TestsQuestions_TestId",
                table: "TestsQuestions",
                column: "TestId");
        }
    }
}
