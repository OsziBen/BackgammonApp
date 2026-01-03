using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameBoardStateAndDiceRollTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardStates");

            migrationBuilder.DropTable(
                name: "DiceRolls");

            migrationBuilder.CreateTable(
                name: "BoardStateSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerTurnId = table.Column<Guid>(type: "uuid", nullable: true),
                    CheckerMoveId = table.Column<Guid>(type: "uuid", nullable: true),
                    PositionsJson = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    BarWhite = table.Column<int>(type: "integer", nullable: false),
                    BarBlack = table.Column<int>(type: "integer", nullable: false),
                    OffWhite = table.Column<int>(type: "integer", nullable: false),
                    OffBlack = table.Column<int>(type: "integer", nullable: false),
                    CurrentPlayer = table.Column<int>(type: "integer", nullable: false),
                    CubeValue = table.Column<int>(type: "integer", nullable: false),
                    CubeOwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardStateSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardStateSnapshots_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiceRollSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerTurnId = table.Column<Guid>(type: "uuid", nullable: false),
                    Die1 = table.Column<int>(type: "integer", nullable: false),
                    Die2 = table.Column<int>(type: "integer", nullable: false),
                    IsDouble = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiceRollSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiceRollSnapshots_PlayerTurns_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardStateSnapshots_GameId_Order",
                table: "BoardStateSnapshots",
                columns: new[] { "GameId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_DiceRollSnapshots_PlayerTurnId",
                table: "DiceRollSnapshots",
                column: "PlayerTurnId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardStateSnapshots");

            migrationBuilder.DropTable(
                name: "DiceRollSnapshots");

            migrationBuilder.CreateTable(
                name: "BoardStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BarBlack = table.Column<int>(type: "integer", nullable: false),
                    BarWhite = table.Column<int>(type: "integer", nullable: false),
                    CheckerMoveId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CubeOwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CubeValue = table.Column<int>(type: "integer", nullable: false),
                    CurrentPlayer = table.Column<int>(type: "integer", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    OffBlack = table.Column<int>(type: "integer", nullable: false),
                    OffWhite = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    PlayerTurnId = table.Column<Guid>(type: "uuid", nullable: true),
                    PositionsJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardStates_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiceRolls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerTurnId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Die1 = table.Column<int>(type: "integer", nullable: false),
                    Die2 = table.Column<int>(type: "integer", nullable: false),
                    IsDouble = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiceRolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiceRolls_PlayerTurns_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardStates_GameId_Order",
                table: "BoardStates",
                columns: new[] { "GameId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_DiceRolls_PlayerTurnId",
                table: "DiceRolls",
                column: "PlayerTurnId",
                unique: true);
        }
    }
}
