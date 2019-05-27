using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Operations.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoutingSlipStates",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: true),
                    FaultSummary = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutingSlipStates", x => x.CorrelationId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutingSlipStates");
        }
    }
}
