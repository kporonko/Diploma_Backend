using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderNumberToQuestionAndOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "QuestionOption",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "QuestionOption");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Question");
        }
    }
}
