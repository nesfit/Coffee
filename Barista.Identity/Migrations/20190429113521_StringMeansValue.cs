using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Identity.Migrations
{
    public partial class StringMeansValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MeansOfAuthentication",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Value",
                table: "MeansOfAuthentication",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
