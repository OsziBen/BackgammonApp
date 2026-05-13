using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueIndexFromJoinRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TournamentJoinRequests_UserId_TournamentId",
                table: "TournamentJoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_GroupJoinRequests_UserId_GroupId",
                table: "GroupJoinRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TournamentJoinRequests_UserId_TournamentId",
                table: "TournamentJoinRequests",
                columns: new[] { "UserId", "TournamentId" },
                unique: true,
                filter: "\"Status\" = 0");

            migrationBuilder.CreateIndex(
                name: "IX_GroupJoinRequests_UserId_GroupId",
                table: "GroupJoinRequests",
                columns: new[] { "UserId", "GroupId" },
                unique: true,
                filter: "\"Status\" = 0");
        }
    }
}
