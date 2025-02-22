using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class updatedBranch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Service_Name",
                schema: "Service",
                table: "Service");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Service",
                table: "Service",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Service",
                table: "Branch",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name_Description",
                schema: "Service",
                table: "Service",
                columns: new[] { "Name", "Description" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Service_Name_Description",
                schema: "Service",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Service",
                table: "Branch");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "Service",
                table: "Service",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                schema: "Service",
                table: "Service",
                column: "Name",
                unique: true);
        }
    }
}
