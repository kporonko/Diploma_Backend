using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserRelationshipsToAllNeededEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UnitSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UnitAppearance",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Targeting",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SurveyUnit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UnitSettings_UserId",
                table: "UnitSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAppearance_UserId",
                table: "UnitAppearance",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Targeting_UserId",
                table: "Targeting",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyUnit_UserId",
                table: "SurveyUnit",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurveyUnit_User_UserId",
                table: "SurveyUnit",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Targeting_User_UserId",
                table: "Targeting",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitAppearance_User_UserId",
                table: "UnitAppearance",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitSettings_User_UserId",
                table: "UnitSettings",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurveyUnit_User_UserId",
                table: "SurveyUnit");

            migrationBuilder.DropForeignKey(
                name: "FK_Targeting_User_UserId",
                table: "Targeting");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitAppearance_User_UserId",
                table: "UnitAppearance");

            migrationBuilder.DropForeignKey(
                name: "FK_UnitSettings_User_UserId",
                table: "UnitSettings");

            migrationBuilder.DropIndex(
                name: "IX_UnitSettings_UserId",
                table: "UnitSettings");

            migrationBuilder.DropIndex(
                name: "IX_UnitAppearance_UserId",
                table: "UnitAppearance");

            migrationBuilder.DropIndex(
                name: "IX_Targeting_UserId",
                table: "Targeting");

            migrationBuilder.DropIndex(
                name: "IX_SurveyUnit_UserId",
                table: "SurveyUnit");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UnitSettings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UnitAppearance");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Targeting");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SurveyUnit");
        }
    }
}
