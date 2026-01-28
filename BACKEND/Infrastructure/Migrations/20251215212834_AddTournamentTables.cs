using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardState_Game_GameId",
                table: "BoardState");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckerMove_PlayerTurn_PlayerTurnId",
                table: "CheckerMove");

            migrationBuilder.DropForeignKey(
                name: "FK_CubeAction_PlayerTurn_PlayerTurnId",
                table: "CubeAction");

            migrationBuilder.DropForeignKey(
                name: "FK_CubeAction_Users_NewOwnerId",
                table: "CubeAction");

            migrationBuilder.DropForeignKey(
                name: "FK_CubeAction_Users_PreviousOwnerId",
                table: "CubeAction");

            migrationBuilder.DropForeignKey(
                name: "FK_DiceRoll_PlayerTurn_PlayerTurnId",
                table: "DiceRoll");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Match_MatchId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Users_DoublingCubeOwnerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Users_StartingPlayerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Game_Users_WinnerId",
                table: "Game");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_RulesTemplate_RulesTemplateId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Users_BlackPlayerId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Users_CreatedByUserId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Users_WhitePlayerId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Users_WinnerId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTurn_Game_GameId",
                table: "PlayerTurn");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTurn_Users_PlayerId",
                table: "PlayerTurn");

            migrationBuilder.DropForeignKey(
                name: "FK_RulesTemplate_Users_AuthorId",
                table: "RulesTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RulesTemplate",
                table: "RulesTemplate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTurn",
                table: "PlayerTurn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Game",
                table: "Game");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiceRoll",
                table: "DiceRoll");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CubeAction",
                table: "CubeAction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckerMove",
                table: "CheckerMove");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardState",
                table: "BoardState");

            migrationBuilder.RenameTable(
                name: "RulesTemplate",
                newName: "RulesTemplates");

            migrationBuilder.RenameTable(
                name: "PlayerTurn",
                newName: "PlayerTurns");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "Game",
                newName: "Games");

            migrationBuilder.RenameTable(
                name: "DiceRoll",
                newName: "DiceRolls");

            migrationBuilder.RenameTable(
                name: "CubeAction",
                newName: "CubeActions");

            migrationBuilder.RenameTable(
                name: "CheckerMove",
                newName: "CheckerMoves");

            migrationBuilder.RenameTable(
                name: "BoardState",
                newName: "BoardStates");

            migrationBuilder.RenameIndex(
                name: "IX_RulesTemplate_AuthorId",
                table: "RulesTemplates",
                newName: "IX_RulesTemplates_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTurn_PlayerId",
                table: "PlayerTurns",
                newName: "IX_PlayerTurns_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTurn_GameId",
                table: "PlayerTurns",
                newName: "IX_PlayerTurns_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_WinnerId",
                table: "Matches",
                newName: "IX_Matches_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_WhitePlayerId",
                table: "Matches",
                newName: "IX_Matches_WhitePlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_RulesTemplateId",
                table: "Matches",
                newName: "IX_Matches_RulesTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_Id_CurrentGameNumber",
                table: "Matches",
                newName: "IX_Matches_Id_CurrentGameNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Match_CreatedByUserId",
                table: "Matches",
                newName: "IX_Matches_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_BlackPlayerId",
                table: "Matches",
                newName: "IX_Matches_BlackPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_WinnerId",
                table: "Games",
                newName: "IX_Games_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_StartingPlayerId",
                table: "Games",
                newName: "IX_Games_StartingPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_MatchId",
                table: "Games",
                newName: "IX_Games_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Game_DoublingCubeOwnerId",
                table: "Games",
                newName: "IX_Games_DoublingCubeOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_DiceRoll_PlayerTurnId",
                table: "DiceRolls",
                newName: "IX_DiceRolls_PlayerTurnId");

            migrationBuilder.RenameIndex(
                name: "IX_CubeAction_PreviousOwnerId",
                table: "CubeActions",
                newName: "IX_CubeActions_PreviousOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CubeAction_PlayerTurnId_OrderWithinTurn",
                table: "CubeActions",
                newName: "IX_CubeActions_PlayerTurnId_OrderWithinTurn");

            migrationBuilder.RenameIndex(
                name: "IX_CubeAction_NewOwnerId",
                table: "CubeActions",
                newName: "IX_CubeActions_NewOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckerMove_PlayerTurnId_OrderWithinTurn",
                table: "CheckerMoves",
                newName: "IX_CheckerMoves_PlayerTurnId_OrderWithinTurn");

            migrationBuilder.RenameIndex(
                name: "IX_BoardState_GameId_Order",
                table: "BoardStates",
                newName: "IX_BoardStates_GameId_Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RulesTemplates",
                table: "RulesTemplates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTurns",
                table: "PlayerTurns",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiceRolls",
                table: "DiceRolls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CubeActions",
                table: "CubeActions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckerMoves",
                table: "CheckerMoves",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardStates",
                table: "BoardStates",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OrganizerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RulesTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_RulesTemplates_RulesTemplateId",
                        column: x => x.RulesTemplateId,
                        principalTable: "RulesTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tournaments_Users_OrganizerUserId",
                        column: x => x.OrganizerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentParticipants_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentParticipants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentRounds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoundNumber = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentRounds_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Seed = table.Column<int>(type: "integer", nullable: true),
                    ConfirmedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentRegistrations_TournamentParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentRegistrations_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentStandings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Wins = table.Column<int>(type: "integer", nullable: false),
                    Losses = table.Column<int>(type: "integer", nullable: false),
                    Draws = table.Column<int>(type: "integer", nullable: false),
                    ByeCount = table.Column<int>(type: "integer", nullable: false),
                    TieBreakScore = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStandings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentStandings_TournamentParticipants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentStandings_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TournamentPairings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TournamentRoundId = table.Column<Guid>(type: "uuid", nullable: false),
                    WhiteParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlackParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Result = table.Column<int>(type: "integer", nullable: true),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecordingUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPairings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentPairings_TournamentParticipants_BlackParticipantId",
                        column: x => x.BlackParticipantId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentPairings_TournamentParticipants_WhiteParticipantId",
                        column: x => x.WhiteParticipantId,
                        principalTable: "TournamentParticipants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentPairings_TournamentRounds_TournamentRoundId",
                        column: x => x.TournamentRoundId,
                        principalTable: "TournamentRounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairings_BlackParticipantId",
                table: "TournamentPairings",
                column: "BlackParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairings_TournamentRoundId",
                table: "TournamentPairings",
                column: "TournamentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPairings_WhiteParticipantId",
                table: "TournamentPairings",
                column: "WhiteParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_TournamentId",
                table: "TournamentParticipants",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_UserId",
                table: "TournamentParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_ParticipantId",
                table: "TournamentRegistrations",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_TournamentId",
                table: "TournamentRegistrations",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRounds_TournamentId",
                table: "TournamentRounds",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_OrganizerUserId",
                table: "Tournaments",
                column: "OrganizerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_RulesTemplateId",
                table: "Tournaments",
                column: "RulesTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStandings_ParticipantId",
                table: "TournamentStandings",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStandings_TournamentId_ParticipantId",
                table: "TournamentStandings",
                columns: new[] { "TournamentId", "ParticipantId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardStates_Games_GameId",
                table: "BoardStates",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckerMoves_PlayerTurns_PlayerTurnId",
                table: "CheckerMoves",
                column: "PlayerTurnId",
                principalTable: "PlayerTurns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CubeActions_PlayerTurns_PlayerTurnId",
                table: "CubeActions",
                column: "PlayerTurnId",
                principalTable: "PlayerTurns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CubeActions_Users_NewOwnerId",
                table: "CubeActions",
                column: "NewOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CubeActions_Users_PreviousOwnerId",
                table: "CubeActions",
                column: "PreviousOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiceRolls_PlayerTurns_PlayerTurnId",
                table: "DiceRolls",
                column: "PlayerTurnId",
                principalTable: "PlayerTurns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Matches_MatchId",
                table: "Games",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_DoublingCubeOwnerId",
                table: "Games",
                column: "DoublingCubeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_StartingPlayerId",
                table: "Games",
                column: "StartingPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Users_WinnerId",
                table: "Games",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_RulesTemplates_RulesTemplateId",
                table: "Matches",
                column: "RulesTemplateId",
                principalTable: "RulesTemplates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_BlackPlayerId",
                table: "Matches",
                column: "BlackPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_CreatedByUserId",
                table: "Matches",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_WhitePlayerId",
                table: "Matches",
                column: "WhitePlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_WinnerId",
                table: "Matches",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTurns_Games_GameId",
                table: "PlayerTurns",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTurns_Users_PlayerId",
                table: "PlayerTurns",
                column: "PlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RulesTemplates_Users_AuthorId",
                table: "RulesTemplates",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX ""IX_Tournament_Organizer_Name_Active""
                  ON ""Tournaments"" (""OrganizerUserId"", ""Name"")
                  WHERE ""IsDeleted"" = FALSE;"
            );

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX ""IX_TournamentParticipant_TournamentId_UserId_Active""
                  ON ""TournamentParticipants"" (""TournamentId"", ""UserId"")
                  WHERE ""UserId"" IS NOT NULL
                  AND ""IsDeleted"" = FALSE;"
            );

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX ""IX_TournamentParticipant_TournamentId_Displayname_Active""
                  ON ""TournamentParticipants"" (""TournamentId"", ""DisplayName"")
                  WHERE ""UserId"" IS NULL
                  AND ""IsDeleted"" = FALSE;"
            );

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX ""IX_TournamentRegistration_TournamentId_ParticipantId_Active"" 
                  ON ""TournamentRegistrations"" (""TournamentId"", ""ParticipantId"")
                  WHERE ""IsDeleted"" = false
                  AND ""Status"" IN (0, 1);"
            );

            migrationBuilder.Sql(
                @"CREATE UNIQUE INDEX ""IX_TournamentRound_TournamentId_RoundNumber_Active""
                  ON ""TournamentRounds"" (""TournamentId"", ""RoundNumber"")
                  WHERE ""IsDeleted"" = FALSE;"
            );

            migrationBuilder.Sql(
                @"
                CREATE INDEX ""IX_TournamentPairing_TournamentRoundId_WhiteParticipantId_Active""
                ON ""TournamentPairings"" (""TournamentRoundId"", ""WhiteParticipantId"")
                WHERE ""IsDeleted"" = FALSE;"
            );

            migrationBuilder.Sql(
                @"CREATE INDEX ""IX_TournamentPairing_TournamentRoundId_BlackParticipantId_Active""
                ON ""TournamentPairings"" (""TournamentRoundId"", ""BlackParticipantId"")
                WHERE ""IsDeleted"" = FALSE;"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardStates_Games_GameId",
                table: "BoardStates");

            migrationBuilder.DropForeignKey(
                name: "FK_CheckerMoves_PlayerTurns_PlayerTurnId",
                table: "CheckerMoves");

            migrationBuilder.DropForeignKey(
                name: "FK_CubeActions_PlayerTurns_PlayerTurnId",
                table: "CubeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_CubeActions_Users_NewOwnerId",
                table: "CubeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_CubeActions_Users_PreviousOwnerId",
                table: "CubeActions");

            migrationBuilder.DropForeignKey(
                name: "FK_DiceRolls_PlayerTurns_PlayerTurnId",
                table: "DiceRolls");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Matches_MatchId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_DoublingCubeOwnerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_StartingPlayerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Users_WinnerId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_RulesTemplates_RulesTemplateId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_BlackPlayerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_CreatedByUserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_WhitePlayerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_WinnerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTurns_Games_GameId",
                table: "PlayerTurns");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTurns_Users_PlayerId",
                table: "PlayerTurns");

            migrationBuilder.DropForeignKey(
                name: "FK_RulesTemplates_Users_AuthorId",
                table: "RulesTemplates");

            migrationBuilder.DropTable(
                name: "TournamentPairings");

            migrationBuilder.DropTable(
                name: "TournamentRegistrations");

            migrationBuilder.DropTable(
                name: "TournamentStandings");

            migrationBuilder.DropTable(
                name: "TournamentRounds");

            migrationBuilder.DropTable(
                name: "TournamentParticipants");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RulesTemplates",
                table: "RulesTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTurns",
                table: "PlayerTurns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiceRolls",
                table: "DiceRolls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CubeActions",
                table: "CubeActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckerMoves",
                table: "CheckerMoves");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BoardStates",
                table: "BoardStates");

            migrationBuilder.RenameTable(
                name: "RulesTemplates",
                newName: "RulesTemplate");

            migrationBuilder.RenameTable(
                name: "PlayerTurns",
                newName: "PlayerTurn");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameTable(
                name: "Games",
                newName: "Game");

            migrationBuilder.RenameTable(
                name: "DiceRolls",
                newName: "DiceRoll");

            migrationBuilder.RenameTable(
                name: "CubeActions",
                newName: "CubeAction");

            migrationBuilder.RenameTable(
                name: "CheckerMoves",
                newName: "CheckerMove");

            migrationBuilder.RenameTable(
                name: "BoardStates",
                newName: "BoardState");

            migrationBuilder.RenameIndex(
                name: "IX_RulesTemplates_AuthorId",
                table: "RulesTemplate",
                newName: "IX_RulesTemplate_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTurns_PlayerId",
                table: "PlayerTurn",
                newName: "IX_PlayerTurn_PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTurns_GameId",
                table: "PlayerTurn",
                newName: "IX_PlayerTurn_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WinnerId",
                table: "Match",
                newName: "IX_Match_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WhitePlayerId",
                table: "Match",
                newName: "IX_Match_WhitePlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_RulesTemplateId",
                table: "Match",
                newName: "IX_Match_RulesTemplateId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Id_CurrentGameNumber",
                table: "Match",
                newName: "IX_Match_Id_CurrentGameNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_CreatedByUserId",
                table: "Match",
                newName: "IX_Match_CreatedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_BlackPlayerId",
                table: "Match",
                newName: "IX_Match_BlackPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_WinnerId",
                table: "Game",
                newName: "IX_Game_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_StartingPlayerId",
                table: "Game",
                newName: "IX_Game_StartingPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_MatchId",
                table: "Game",
                newName: "IX_Game_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_DoublingCubeOwnerId",
                table: "Game",
                newName: "IX_Game_DoublingCubeOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_DiceRolls_PlayerTurnId",
                table: "DiceRoll",
                newName: "IX_DiceRoll_PlayerTurnId");

            migrationBuilder.RenameIndex(
                name: "IX_CubeActions_PreviousOwnerId",
                table: "CubeAction",
                newName: "IX_CubeAction_PreviousOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CubeActions_PlayerTurnId_OrderWithinTurn",
                table: "CubeAction",
                newName: "IX_CubeAction_PlayerTurnId_OrderWithinTurn");

            migrationBuilder.RenameIndex(
                name: "IX_CubeActions_NewOwnerId",
                table: "CubeAction",
                newName: "IX_CubeAction_NewOwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_CheckerMoves_PlayerTurnId_OrderWithinTurn",
                table: "CheckerMove",
                newName: "IX_CheckerMove_PlayerTurnId_OrderWithinTurn");

            migrationBuilder.RenameIndex(
                name: "IX_BoardStates_GameId_Order",
                table: "BoardState",
                newName: "IX_BoardState_GameId_Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RulesTemplate",
                table: "RulesTemplate",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTurn",
                table: "PlayerTurn",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Game",
                table: "Game",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiceRoll",
                table: "DiceRoll",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CubeAction",
                table: "CubeAction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckerMove",
                table: "CheckerMove",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BoardState",
                table: "BoardState",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardState_Game_GameId",
                table: "BoardState",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CheckerMove_PlayerTurn_PlayerTurnId",
                table: "CheckerMove",
                column: "PlayerTurnId",
                principalTable: "PlayerTurn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CubeAction_PlayerTurn_PlayerTurnId",
                table: "CubeAction",
                column: "PlayerTurnId",
                principalTable: "PlayerTurn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CubeAction_Users_NewOwnerId",
                table: "CubeAction",
                column: "NewOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CubeAction_Users_PreviousOwnerId",
                table: "CubeAction",
                column: "PreviousOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DiceRoll_PlayerTurn_PlayerTurnId",
                table: "DiceRoll",
                column: "PlayerTurnId",
                principalTable: "PlayerTurn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Match_MatchId",
                table: "Game",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Users_DoublingCubeOwnerId",
                table: "Game",
                column: "DoublingCubeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Users_StartingPlayerId",
                table: "Game",
                column: "StartingPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Game_Users_WinnerId",
                table: "Game",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_RulesTemplate_RulesTemplateId",
                table: "Match",
                column: "RulesTemplateId",
                principalTable: "RulesTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Users_BlackPlayerId",
                table: "Match",
                column: "BlackPlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Users_CreatedByUserId",
                table: "Match",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Users_WhitePlayerId",
                table: "Match",
                column: "WhitePlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Users_WinnerId",
                table: "Match",
                column: "WinnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTurn_Game_GameId",
                table: "PlayerTurn",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTurn_Users_PlayerId",
                table: "PlayerTurn",
                column: "PlayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RulesTemplate_Users_AuthorId",
                table: "RulesTemplate",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_Tournament_Organizer_Name_Active"";"
            );

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_TournamentParticipant_TournamentId_UserId_Active"";"
            );

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_TournamentParticipant_TournamentId_Displayname_Active"";"
            );

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_TournamentRegistration_TournamentId_ParticipantId_Active"";"
            );

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_TournamentRound_TournamentId_RoundNumber_Active"";"
            );

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_TournamentPairing_TournamentRoundId_WhiteParticipantId_Active"";"
            );

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_TournamentPairing_TournamentRoundId_BlackParticipantId_Active"";"
            );
        }
    }
}
