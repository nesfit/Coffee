using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.PointsOfSale.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointsOfSale",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    ParentAccountingGroupId = table.Column<Guid>(nullable: false),
                    SaleStrategyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsOfSale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthorizations",
                columns: table => new
                {
                    PointOfSaleId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    PointOfSaleId1 = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizations", x => new { x.PointOfSaleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserAuthorizations_PointsOfSale_PointOfSaleId1",
                        column: x => x.PointOfSaleId1,
                        principalTable: "PointsOfSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizations_PointOfSaleId1",
                table: "UserAuthorizations",
                column: "PointOfSaleId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAuthorizations");

            migrationBuilder.DropTable(
                name: "PointsOfSale");
        }
    }
}
