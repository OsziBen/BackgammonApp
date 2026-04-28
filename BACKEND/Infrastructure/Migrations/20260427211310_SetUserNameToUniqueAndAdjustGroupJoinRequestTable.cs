using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetUserNameToUniqueAndAdjustGroupJoinRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewedBy",
                table: "GroupJoinRequests",
                newName: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupJoinRequests_ReviewedByUserId",
                table: "GroupJoinRequests",
                column: "ReviewedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupJoinRequests_Users_ReviewedByUserId",
                table: "GroupJoinRequests",
                column: "ReviewedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupJoinRequests_Users_ReviewedByUserId",
                table: "GroupJoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_GroupJoinRequests_ReviewedByUserId",
                table: "GroupJoinRequests");

            migrationBuilder.RenameColumn(
                name: "ReviewedByUserId",
                table: "GroupJoinRequests",
                newName: "ReviewedBy");
        }
    }
}
