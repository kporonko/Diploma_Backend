using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NameAddedToUnitAppearance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UnitAppearance",
                type: "varchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UnitAppearance");
        }
    }
}
