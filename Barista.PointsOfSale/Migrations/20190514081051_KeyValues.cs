using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.PointsOfSale.Migrations
{
    public partial class KeyValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PointOfSaleKeyValue",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(nullable: false),
                    PointOfSaleId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointOfSaleKeyValue", x => new { x.Id, x.Key });
                    table.ForeignKey(
                        name: "FK_PointOfSaleKeyValue_PointsOfSale_PointOfSaleId",
                        column: x => x.PointOfSaleId,
                        principalTable: "PointsOfSale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PointOfSaleKeyValue_PointOfSaleId",
                table: "PointOfSaleKeyValue",
                column: "PointOfSaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PointOfSaleKeyValue");
        }
    }
}
