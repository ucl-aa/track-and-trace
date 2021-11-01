using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZipCodes",
                columns: table => new
                {
                    ZipCodeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCodeValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZipCodes", x => x.ZipCodeId);
                });

            migrationBuilder.CreateTable(
                name: "DropOffs",
                columns: table => new
                {
                    Key = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    ZipCodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Terminal = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropOffs", x => x.Key);
                    table.ForeignKey(
                        name: "FK_DropOffs_ZipCodes_ZipCodeId",
                        column: x => x.ZipCodeId,
                        principalTable: "ZipCodes",
                        principalColumn: "ZipCodeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TracingId = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DropOffKey = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deliveries_DropOffs_DropOffKey",
                        column: x => x.DropOffKey,
                        principalTable: "DropOffs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UpdateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    DeliveryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_DropOffKey",
                table: "Deliveries",
                column: "DropOffKey");

            migrationBuilder.CreateIndex(
                name: "IX_DropOffs_ZipCodeId",
                table: "DropOffs",
                column: "ZipCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Status_DeliveryId",
                table: "Status",
                column: "DeliveryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "DropOffs");

            migrationBuilder.DropTable(
                name: "ZipCodes");
        }
    }
}
