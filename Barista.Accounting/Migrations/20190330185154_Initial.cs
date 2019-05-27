using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Accounting.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    Quantity = table.Column<decimal>(nullable: false),
                    AccountingGroupId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    AuthenticationMeansId = table.Column<Guid>(nullable: false),
                    PointOfSaleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleStateChange",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    CausedByPointOfSaleId = table.Column<Guid>(nullable: true),
                    CausedByUserId = table.Column<Guid>(nullable: true),
                    SaleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleStateChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleStateChange_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_AccountingGroupId",
                table: "Sales",
                column: "AccountingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PointOfSaleId",
                table: "Sales",
                column: "PointOfSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_UserId",
                table: "Sales",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleStateChange_SaleId",
                table: "SaleStateChange",
                column: "SaleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "SaleStateChange");

            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
