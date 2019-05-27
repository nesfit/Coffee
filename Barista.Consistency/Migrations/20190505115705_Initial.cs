using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Consistency.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    MessageTypeName = table.Column<string>(nullable: true),
                    SerializedContents = table.Column<string>(nullable: true),
                    ScheduledFor = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledEvents", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledEvents_ScheduledFor",
                table: "ScheduledEvents",
                column: "ScheduledFor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledEvents");
        }
    }
}
