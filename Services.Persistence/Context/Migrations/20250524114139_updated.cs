using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.Persistence.Context.Migrations
{
    /// <inheritdoc />
    public partial class updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "Service",
                table: "Booking",
                newName: "UpdatedPrice");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                schema: "Service",
                table: "Booking",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "OldPrice",
                schema: "Service",
                table: "Booking",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "DiscountRule",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MainPoints = table.Column<int>(type: "int", nullable: false),
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountRule_Discount_DiscountId",
                        column: x => x.DiscountId,
                        principalSchema: "Service",
                        principalTable: "Discount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Point",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Point_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Service",
                        principalTable: "Customer",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRule_DiscountId",
                schema: "Service",
                table: "DiscountRule",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Point_CustomerId",
                schema: "Service",
                table: "Point",
                column: "CustomerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountRule",
                schema: "Service");

            migrationBuilder.DropTable(
                name: "Point",
                schema: "Service");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                schema: "Service",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "OldPrice",
                schema: "Service",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "UpdatedPrice",
                schema: "Service",
                table: "Booking",
                newName: "Price");
        }
    }
}
