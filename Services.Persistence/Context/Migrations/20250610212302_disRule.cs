using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class disRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Branch_UserId",
                schema: "Service",
                table: "Branch");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_UserId",
                schema: "Service",
                table: "Branch",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Branch_UserId",
                schema: "Service",
                table: "Branch");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_UserId",
                schema: "Service",
                table: "Branch",
                column: "UserId",
                unique: true);
        }
    }
}
