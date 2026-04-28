using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdjustGroupProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_CreatorId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupType",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "JoinPolicy",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MaxModerators",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "SizePreset",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "Visibility",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "SystemName",
                table: "GroupRoles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "GroupRoles",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "SystemName",
                value: "OWNER");

            migrationBuilder.UpdateData(
                table: "GroupRoles",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "SystemName",
                value: "MODERATOR");

            migrationBuilder.UpdateData(
                table: "GroupRoles",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "SystemName",
                value: "MEMBER");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatorId_Name",
                table: "Groups",
                columns: new[] { "CreatorId", "Name" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Groups_CreatorId_Name",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "JoinPolicy",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "MaxModerators",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SizePreset",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Visibility",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SystemName",
                table: "GroupRoles");

            migrationBuilder.AddColumn<int>(
                name: "GroupType",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CreatorId",
                table: "Groups",
                column: "CreatorId");
        }
    }
}
