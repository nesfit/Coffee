using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.AccountingGroups.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorizations_AccountingGroups_AccountingGroupId1",
                table: "UserAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthorizations_AccountingGroupId1",
                table: "UserAuthorizations");

            migrationBuilder.DropColumn(
                name: "AccountingGroupId1",
                table: "UserAuthorizations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountingGroupId1",
                table: "UserAuthorizations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizations_AccountingGroupId1",
                table: "UserAuthorizations",
                column: "AccountingGroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorizations_AccountingGroups_AccountingGroupId1",
                table: "UserAuthorizations",
                column: "AccountingGroupId1",
                principalTable: "AccountingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
