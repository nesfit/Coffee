using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Barista.Consistency.Migrations
{
    public partial class AdminAccountCreationEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScheduledEvents",
                columns: new[] { "Id", "Created", "MessageTypeName", "ScheduledFor", "SerializedContents", "Updated" },
                values: new object[] { new Guid("02070c27-4939-4d94-96e4-463ff617ff18"), new DateTimeOffset(new DateTime(2019, 5, 27, 13, 49, 40, 264, DateTimeKind.Unspecified).AddTicks(8266), new TimeSpan(0, 0, 0, 0, 0)), "Barista.Contracts.Events.Consistency.IDatabaseCreated, Barista.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", new DateTimeOffset(new DateTime(2019, 5, 27, 13, 49, 40, 264, DateTimeKind.Unspecified).AddTicks(6654), new TimeSpan(0, 0, 0, 0, 0)), "{}", new DateTimeOffset(new DateTime(2019, 5, 27, 13, 49, 40, 264, DateTimeKind.Unspecified).AddTicks(8268), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScheduledEvents",
                keyColumn: "Id",
                keyValue: new Guid("02070c27-4939-4d94-96e4-463ff617ff18"));
        }
    }
}
