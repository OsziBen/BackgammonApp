using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustGroupMembershipModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupMemberships_UserId_GroupId",
                table: "GroupMemberships");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DisabledAt",
                table: "GroupMemberships",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GroupMemberships",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberships_UserId_GroupId",
                table: "GroupMemberships",
                columns: new[] { "UserId", "GroupId" },
                unique: true,
                filter: "\"IsActive\" = true");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GroupMemberships_UserId_GroupId",
                table: "GroupMemberships");

            migrationBuilder.DropColumn(
                name: "DisabledAt",
                table: "GroupMemberships");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GroupMemberships");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberships_UserId_GroupId",
                table: "GroupMemberships",
                columns: new[] { "UserId", "GroupId" },
                unique: true);
        }
    }
}
