using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustTournamentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentRegistrations_Tournaments_TournamentId",
                table: "TournamentRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TournamentRegistrations_ParticipantId",
                table: "TournamentRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TournamentRegistrations_TournamentId",
                table: "TournamentRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TournamentParticipants_TournamentId",
                table: "TournamentParticipants");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "TournamentRegistrations");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TournamentParticipants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Visibility",
                table: "Groups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "SizePreset",
                table: "Groups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "JoinPolicy",
                table: "Groups",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 1);

            migrationBuilder.InsertData(
                table: "RulesTemplates",
                columns: new[] { "Id", "AuthorId", "CrawfordRuleEnabled", "CreatedAt", "DeletedAt", "Description", "IsPublic", "LastUpdatedAt", "MatchTimePerPlayerInSeconds", "Name", "StartOfTurnDelayPerPlayerInSeconds", "TargetScore" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, true, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, true, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Standard 5 points", null, 5 },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, true, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, true, new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Standard 7 points", null, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_ParticipantId",
                table: "TournamentRegistrations",
                column: "ParticipantId",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_TournamentId_UserId",
                table: "TournamentParticipants",
                columns: new[] { "TournamentId", "UserId" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentRegistrations_ParticipantId",
                table: "TournamentRegistrations");

            migrationBuilder.DropIndex(
                name: "IX_TournamentParticipants_TournamentId_UserId",
                table: "TournamentParticipants");

            migrationBuilder.DeleteData(
                table: "RulesTemplates",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "RulesTemplates",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentId",
                table: "TournamentRegistrations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TournamentParticipants",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "Visibility",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "SizePreset",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "JoinPolicy",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_ParticipantId",
                table: "TournamentRegistrations",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentRegistrations_TournamentId",
                table: "TournamentRegistrations",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentParticipants_TournamentId",
                table: "TournamentParticipants",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentRegistrations_Tournaments_TournamentId",
                table: "TournamentRegistrations",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
