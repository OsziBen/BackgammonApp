using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWinnerPlayerToGameSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WinnerPlayerId",
                table: "GameSessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_WinnerPlayerId",
                table: "GameSessions",
                column: "WinnerPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GamePlayers_WinnerPlayerId",
                table: "GameSessions",
                column: "WinnerPlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GamePlayers_WinnerPlayerId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_WinnerPlayerId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "WinnerPlayerId",
                table: "GameSessions");
        }
    }
}
