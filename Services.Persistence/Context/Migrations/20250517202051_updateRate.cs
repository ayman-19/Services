using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class updateRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_Id",
                schema: "Service",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Identity",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "Rate",
                schema: "Service",
                table: "Booking",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                schema: "Identity",
                table: "User",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CreateOn",
                schema: "Service",
                table: "Booking",
                column: "CreateOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Name",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CreateOn",
                schema: "Service",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Rate",
                schema: "Service",
                table: "Booking",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_Id",
                schema: "Service",
                table: "Booking",
                column: "Id",
                unique: true);
        }
    }
}
