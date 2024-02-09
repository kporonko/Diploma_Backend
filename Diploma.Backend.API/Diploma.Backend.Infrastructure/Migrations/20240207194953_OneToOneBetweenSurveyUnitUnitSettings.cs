using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OneToOneBetweenSurveyUnitUnitSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUnit_UnitSettings_SettingsId",
                table: "SurveyUnit");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUnit_SettingsId",
                table: "SurveyUnit");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "SurveyUnit");

            migrationBuilder.AddColumn<int>(
                name: "SurveyUnitId",
                table: "UnitSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UnitSettings_SurveyUnitId",
                table: "UnitSettings",
                column: "SurveyUnitId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UnitSettings_SurveyUnit_SurveyUnitId",
                table: "UnitSettings",
                column: "SurveyUnitId",
                principalTable: "SurveyUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitSettings_SurveyUnit_SurveyUnitId",
                table: "UnitSettings");

            migrationBuilder.DropIndex(
                name: "IX_UnitSettings_SurveyUnitId",
                table: "UnitSettings");

            migrationBuilder.DropColumn(
                name: "SurveyUnitId",
                table: "UnitSettings");

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "SurveyUnit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUnit_SettingsId",
                table: "SurveyUnit",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUnit_UnitSettings_SettingsId",
                table: "SurveyUnit",
                column: "SettingsId",
                principalTable: "UnitSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
