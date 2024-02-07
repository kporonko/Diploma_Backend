using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diploma.Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StateAddedToUnitAppearance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "UnitAppearance",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "UnitAppearance");
        }
    }
}
