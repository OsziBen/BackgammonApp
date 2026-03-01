using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFinishReasonAndOutcomePropertiesToGameSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "FinalOutcome_Points",
                table: "GameSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalOutcome_ResultType",
                table: "GameSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinishReason",
                table: "GameSessions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameSessions_CreatedByUserId",
                table: "GameSessions",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_Users_CreatedByUserId",
                table: "GameSessions",
                column: "CreatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_Users_CreatedByUserId",
                table: "GameSessions");

            migrationBuilder.DropIndex(
                name: "IX_GameSessions_CreatedByUserId",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "FinalOutcome_Points",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "FinalOutcome_ResultType",
                table: "GameSessions");

            migrationBuilder.DropColumn(
                name: "FinishReason",
                table: "GameSessions");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePictureUrl",
                table: "Users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
