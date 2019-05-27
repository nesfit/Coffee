using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.PointsOfSale.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuthorizations_PointsOfSale_PointOfSaleId1",
                table: "UserAuthorizations");

            migrationBuilder.DropIndex(
                name: "IX_UserAuthorizations_PointOfSaleId1",
                table: "UserAuthorizations");

            migrationBuilder.DropColumn(
                name: "PointOfSaleId1",
                table: "UserAuthorizations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PointOfSaleId1",
                table: "UserAuthorizations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizations_PointOfSaleId1",
                table: "UserAuthorizations",
                column: "PointOfSaleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuthorizations_PointsOfSale_PointOfSaleId1",
                table: "UserAuthorizations",
                column: "PointOfSaleId1",
                principalTable: "PointsOfSale",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
