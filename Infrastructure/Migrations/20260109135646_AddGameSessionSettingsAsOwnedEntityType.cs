using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGameSessionSettingsAsOwnedEntityType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Settings_ClockEnabled",
                table: "GameSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Settings_CrawfordRuleEnabled",
                table: "GameSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Settings_DoublingCubeEnabled",
                table: "GameSessions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Settings_MatchTimePerPlayerInSeconds",
                table: "GameSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_StartOfTurnDelayPerPlayerInSeconds",
                table: "GameSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Settings_TargerPoints",
                table: "GameSessions",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Settings_ClockEnabled",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Settings_CrawfordRuleEnabled",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Settings_DoublingCubeEnabled",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Settings_MatchTimePerPlayerInSeconds",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Settings_StartOfTurnDelayPerPlayerInSeconds",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "Settings_TargerPoints",
                table: "GameSessions");
        }
    }
}
