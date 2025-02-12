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
            migrationBuilder.EnsureSchema(
                name: "Service");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                schema: "Identity",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Branch",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Langitude = table.Column<double>(type: "float", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Service",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Customer_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                schema: "Service",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Experience = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Worker_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBranch",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerBranch_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Service",
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerBranch_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Service",
                        principalTable: "Customer",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerService",
                schema: "Service",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Availabilty = table.Column<bool>(type: "bit", nullable: false),
                    CreateOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerService_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Service",
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerService_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "Service",
                        principalTable: "Service",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerService_Worker_WorkerId",
                        column: x => x.WorkerId,
                        principalSchema: "Service",
                        principalTable: "Worker",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBranch_BranchId_CustomerId",
                schema: "Service",
                table: "CustomerBranch",
                columns: new[] { "BranchId", "CustomerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBranch_CustomerId",
                schema: "Service",
                table: "CustomerBranch",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                schema: "Service",
                table: "Service",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerService_BranchId",
                schema: "Service",
                table: "WorkerService",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerService_ServiceId_BranchId_WorkerId",
                schema: "Service",
                table: "WorkerService",
                columns: new[] { "ServiceId", "BranchId", "WorkerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerService_WorkerId",
                schema: "Service",
                table: "WorkerService",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerBranch",
                schema: "Service");

            migrationBuilder.DropTable(
                name: "WorkerService",
                schema: "Service");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Service");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "Service");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Service");

            migrationBuilder.DropTable(
                name: "Worker",
                schema: "Service");

            migrationBuilder.DropColumn(
                name: "UserType",
                schema: "Identity",
                table: "User");
        }
    }
}
