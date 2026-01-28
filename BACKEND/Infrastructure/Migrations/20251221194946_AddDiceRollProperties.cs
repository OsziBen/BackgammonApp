using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDiceRollProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "LastDiceRoll",
                table: "GameSessions",
                type: "integer[]",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainingMoves",
                table: "GameSessions",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDiceRoll",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "RemainingMoves",
                table: "GameSessions");
        }
    }
}
