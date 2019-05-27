using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.StockItems.Migrations
{
    public partial class PosId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PointOfSaleId",
                table: "StockItems",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointOfSaleId",
                table: "StockItems");
        }
    }
}
