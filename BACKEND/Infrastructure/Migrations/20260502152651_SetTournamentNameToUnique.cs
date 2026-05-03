using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetTournamentNameToUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tournaments_OrganizerUserId",
                table: "Tournaments");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_OrganizerUserId_Name",
                table: "Tournaments",
                columns: new[] { "OrganizerUserId", "Name" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tournaments_OrganizerUserId_Name",
                table: "Tournaments");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_OrganizerUserId",
                table: "Tournaments",
                column: "OrganizerUserId");
        }
    }
}
