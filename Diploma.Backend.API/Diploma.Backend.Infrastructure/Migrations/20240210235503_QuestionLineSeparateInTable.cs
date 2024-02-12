using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuestionLineSeparateInTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionLine",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "QuestionLineId",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionLine_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionTranslationLine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionLineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionTranslation_QuestionLine_QuestionLineId",
                        column: x => x.QuestionLineId,
                        principalTable: "QuestionLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionLine_QuestionId",
                table: "QuestionLine",
                column: "QuestionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTranslation_QuestionLineId",
                table: "QuestionTranslation",
                column: "QuestionLineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTranslation");

            migrationBuilder.DropTable(
                name: "QuestionLine");

            migrationBuilder.DropColumn(
                name: "QuestionLineId",
                table: "Question");

            migrationBuilder.AddColumn<string>(
                name: "QuestionLine",
                table: "Question",
                type: "varchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
