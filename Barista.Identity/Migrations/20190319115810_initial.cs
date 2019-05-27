using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Identity.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    MeansId = table.Column<Guid>(nullable: false),
                    ValidSince = table.Column<DateTimeOffset>(nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    PointOfSaleId = table.Column<Guid>(nullable: true),
                    UserId = table.Column<Guid>(nullable: true),
                    IsShared = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeansOfAuthentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Value = table.Column<byte[]>(nullable: true),
                    ValidSince = table.Column<DateTimeOffset>(nullable: false),
                    ValidUntil = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeansOfAuthentication", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpendingLimit",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTimeOffset>(nullable: false),
                    Updated = table.Column<DateTimeOffset>(nullable: false),
                    Interval = table.Column<TimeSpan>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    AssignmentToUserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpendingLimit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpendingLimit_Assignments_AssignmentToUserId",
                        column: x => x.AssignmentToUserId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpendingLimit_AssignmentToUserId",
                table: "SpendingLimit",
                column: "AssignmentToUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeansOfAuthentication");

            migrationBuilder.DropTable(
                name: "SpendingLimit");

            migrationBuilder.DropTable(
                name: "Assignments");
        }
    }
}
