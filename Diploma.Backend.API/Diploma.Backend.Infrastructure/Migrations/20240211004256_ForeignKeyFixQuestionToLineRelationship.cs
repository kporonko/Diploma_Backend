using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyFixQuestionToLineRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionLineId",
                table: "Question");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionLineId",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
