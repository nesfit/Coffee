using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Identity.Migrations
{
    public partial class RenameOfMeansTypeAndAdditionOfLabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "MeansOfAuthentication",
                newName: "Method");

            migrationBuilder.AlterColumn<string>(
                name: "Method",
                table: "MeansOfAuthentication",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "MeansOfAuthentication",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeansOfAuthentication_Method",
                table: "MeansOfAuthentication",
                column: "Method");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeansOfAuthentication_Method",
                table: "MeansOfAuthentication");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "MeansOfAuthentication");

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "MeansOfAuthentication",
                newName: "Type");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "MeansOfAuthentication",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
