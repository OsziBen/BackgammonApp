using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixGroupMembershipRolePrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembershipRoles",
                table: "GroupMembershipRoles");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "GroupMembershipRoles",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembershipRoles",
                table: "GroupMembershipRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembershipRoles_GroupMembershipId_GroupRoleId",
                table: "GroupMembershipRoles",
                columns: new[] { "GroupMembershipId", "GroupRoleId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembershipRoles",
                table: "GroupMembershipRoles");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembershipRoles_GroupMembershipId_GroupRoleId",
                table: "GroupMembershipRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GroupMembershipRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembershipRoles",
                table: "GroupMembershipRoles",
                columns: new[] { "GroupMembershipId", "GroupRoleId" });
        }
    }
}
