using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetNullDeleteUnitApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUnit_UnitAppearance_AppearanceId",
                table: "SurveyUnit");

            migrationBuilder.AlterColumn<int>(
                name: "AppearanceId",
                table: "SurveyUnit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUnit_UnitAppearance_AppearanceId",
                table: "SurveyUnit",
                column: "AppearanceId",
                principalTable: "UnitAppearance",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUnit_UnitAppearance_AppearanceId",
                table: "SurveyUnit");

            migrationBuilder.AlterColumn<int>(
                name: "AppearanceId",
                table: "SurveyUnit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUnit_UnitAppearance_AppearanceId",
                table: "SurveyUnit",
                column: "AppearanceId",
                principalTable: "UnitAppearance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
