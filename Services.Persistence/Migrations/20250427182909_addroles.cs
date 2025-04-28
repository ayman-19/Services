using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Role",
                columns: new[] { "Id", "CreateOn", "Name", "UpdateOn" },
                values: new object[,]
                {
                    { new Guid("36d69680-e3b2-4153-b3de-b5b3a537e822"), new DateTime(2025, 4, 27, 18, 29, 8, 0, DateTimeKind.Unspecified), "Worker", null },
                    { new Guid("ae53d444-f81d-490c-b6f6-e5d45baf6619"), new DateTime(2025, 4, 27, 18, 29, 8, 0, DateTimeKind.Unspecified), "Customer", null },
                    { new Guid("d9d07e43-cfc7-4592-8282-c2f422bf0872"), new DateTime(2025, 4, 27, 18, 29, 8, 0, DateTimeKind.Unspecified), "Admin", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("36d69680-e3b2-4153-b3de-b5b3a537e822"));

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("ae53d444-f81d-490c-b6f6-e5d45baf6619"));

            migrationBuilder.DeleteData(
                schema: "Identity",
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d9d07e43-cfc7-4592-8282-c2f422bf0872"));
        }
    }
}
