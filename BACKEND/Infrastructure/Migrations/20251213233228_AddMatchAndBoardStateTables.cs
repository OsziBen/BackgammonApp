using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchAndBoardStateTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RulesTemplate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    AuthorId = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetScore = table.Column<int>(type: "integer", nullable: false),
                    UseClock = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MatchTimePerPlayerInSeconds = table.Column<int>(type: "integer", nullable: true),
                    StartOfTurnDelayPerPlayerInSeconds = table.Column<int>(type: "integer", nullable: true),
                    CrawfordRuleEnabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsPublic = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RulesTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RulesTemplate_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    StatusType = table.Column<int>(type: "integer", nullable: false),
                    WhitePlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlackPlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    RulesTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    TargetScore = table.Column<int>(type: "integer", nullable: false),
                    UseClock = table.Column<bool>(type: "boolean", nullable: false),
                    MatchTimePerPlayerInSeconds = table.Column<int>(type: "integer", nullable: true),
                    StartOfTurnDelayPerPlayerInSeconds = table.Column<int>(type: "integer", nullable: true),
                    CrawfordRuleEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CurrentGameNumber = table.Column<int>(type: "integer", nullable: false),
                    IsResigned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MatchCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    TournamentMatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScheduledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_RulesTemplate_RulesTemplateId",
                        column: x => x.RulesTemplateId,
                        principalTable: "RulesTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Users_BlackPlayerId",
                        column: x => x.BlackPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Users_WhitePlayerId",
                        column: x => x.WhitePlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    GameNumber = table.Column<int>(type: "integer", nullable: false),
                    StartingPlayerId = table.Column<Guid>(type: "uuid", nullable: true),
                    WinnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    WhitePlayerRemainingSeconds = table.Column<int>(type: "integer", nullable: false),
                    BlackPlayerRemainingSeconds = table.Column<int>(type: "integer", nullable: false),
                    WhitePlayerScore = table.Column<int>(type: "integer", nullable: true),
                    BlackPlayerScore = table.Column<int>(type: "integer", nullable: true),
                    DoublingCubeOwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    DoublingCubeValue = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    IsCrawfordActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsTimeOutLoss = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    StartedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_Users_DoublingCubeOwnerId",
                        column: x => x.DoublingCubeOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_Users_StartingPlayerId",
                        column: x => x.StartingPlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardState",
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
                    table.PrimaryKey("PK_BoardState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardState_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTurn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    MoveNumber = table.Column<int>(type: "integer", nullable: false),
                    ResultType = table.Column<int>(type: "integer", nullable: false),
                    WasCubeAvailableToPlayer = table.Column<bool>(type: "boolean", nullable: false),
                    CubeOwnerAtStart = table.Column<Guid>(type: "uuid", nullable: true),
                    CubeOwnerAtEnd = table.Column<Guid>(type: "uuid", nullable: true),
                    CubeValueAtStart = table.Column<int>(type: "integer", nullable: false),
                    CubeValueAtEnd = table.Column<int>(type: "integer", nullable: false),
                    ResultingBoardStateId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTurn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTurn_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerTurn_Users_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckerMove",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerTurnId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderWithinTurn = table.Column<int>(type: "integer", nullable: false),
                    FromPoint = table.Column<int>(type: "integer", nullable: false),
                    ToPoint = table.Column<int>(type: "integer", nullable: false),
                    PipsUsed = table.Column<int>(type: "integer", nullable: false),
                    IsHit = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsBearOff = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ActualBoardStateId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckerMove", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckerMove_PlayerTurn_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubeAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerTurnId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderWithinTurn = table.Column<int>(type: "integer", nullable: false),
                    ActionType = table.Column<int>(type: "integer", nullable: false),
                    PreviousOwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    NewOwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    PreviousCubeValue = table.Column<int>(type: "integer", nullable: false),
                    NewCubeValue = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubeAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubeAction_PlayerTurn_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubeAction_Users_NewOwnerId",
                        column: x => x.NewOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubeAction_Users_PreviousOwnerId",
                        column: x => x.PreviousOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiceRoll",
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
                    table.PrimaryKey("PK_DiceRoll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiceRoll_PlayerTurn_PlayerTurnId",
                        column: x => x.PlayerTurnId,
                        principalTable: "PlayerTurn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardState_GameId_Order",
                table: "BoardState",
                columns: new[] { "GameId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_CheckerMove_PlayerTurnId_OrderWithinTurn",
                table: "CheckerMove",
                columns: new[] { "PlayerTurnId", "OrderWithinTurn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CubeAction_NewOwnerId",
                table: "CubeAction",
                column: "NewOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CubeAction_PlayerTurnId_OrderWithinTurn",
                table: "CubeAction",
                columns: new[] { "PlayerTurnId", "OrderWithinTurn" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CubeAction_PreviousOwnerId",
                table: "CubeAction",
                column: "PreviousOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_DiceRoll_PlayerTurnId",
                table: "DiceRoll",
                column: "PlayerTurnId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_DoublingCubeOwnerId",
                table: "Game",
                column: "DoublingCubeOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_MatchId",
                table: "Game",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_StartingPlayerId",
                table: "Game",
                column: "StartingPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_WinnerId",
                table: "Game",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_BlackPlayerId",
                table: "Match",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_CreatedByUserId",
                table: "Match",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Id_CurrentGameNumber",
                table: "Match",
                columns: new[] { "Id", "CurrentGameNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Match_RulesTemplateId",
                table: "Match",
                column: "RulesTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_WhitePlayerId",
                table: "Match",
                column: "WhitePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_WinnerId",
                table: "Match",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurn_GameId",
                table: "PlayerTurn",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTurn_PlayerId",
                table: "PlayerTurn",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_RulesTemplate_AuthorId",
                table: "RulesTemplate",
                column: "AuthorId");

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX ""IX_Match_MatchCode"" 
                  ON ""Match"" (""MatchCode"") 
                  WHERE ""MatchCode"" IS NOT NULL;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardState");

            migrationBuilder.DropTable(
                name: "CheckerMove");

            migrationBuilder.DropTable(
                name: "CubeAction");

            migrationBuilder.DropTable(
                name: "DiceRoll");

            migrationBuilder.DropTable(
                name: "PlayerTurn");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "RulesTemplate");

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_Match_MatchCode"";"
            );
        }
    }
}
