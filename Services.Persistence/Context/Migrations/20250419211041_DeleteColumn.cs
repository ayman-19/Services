using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class DeleteColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                schema: "Service",
                table: "Worker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                schema: "Service",
                table: "Worker",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
