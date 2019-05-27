using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.AccountingGroups.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountingGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    SaleStrategyId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthorizations",
                columns: table => new
                {
                    AccountingGroupId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    AccountingGroupId1 = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizations", x => new { x.AccountingGroupId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserAuthorizations_AccountingGroups_AccountingGroupId1",
                        column: x => x.AccountingGroupId1,
                        principalTable: "AccountingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizations_AccountingGroupId1",
                table: "UserAuthorizations",
                column: "AccountingGroupId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthorizations");

            migrationBuilder.DropTable(
                name: "AccountingGroups");
        }
    }
}
