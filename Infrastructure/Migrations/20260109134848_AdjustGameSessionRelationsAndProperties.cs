using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustGameSessionRelationsAndProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingMoves",
                table: "GameSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "GameSessions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentGameId",
                table: "GameSessions",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "DoublingCubeValue",
                table: "GameSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DoublingCubeOwnerPlayerId",
                table: "GameSessions",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoublingCubeValue",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "DoublingCubeOwnerPlayerId",
                table: "GameSessions");

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "GameSessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentGameId",
                table: "GameSessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemainingMoves",
                table: "GameSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
