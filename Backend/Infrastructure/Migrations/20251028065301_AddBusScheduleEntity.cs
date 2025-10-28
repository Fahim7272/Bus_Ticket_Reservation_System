using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBusScheduleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DroppingPoint",
                table: "Tickets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BoardingPoint",
                table: "Tickets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "BusScheduleId",
                table: "Tickets",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "StopName",
                table: "RouteStops",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BusNumber",
                table: "Buses",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "BusSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BusId = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    JourneyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusSchedules_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusSchedules_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_BusScheduleId",
                table: "Tickets",
                column: "BusScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_MobileNumber",
                table: "Passengers",
                column: "MobileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_BusId",
                table: "BusSchedules",
                column: "BusId");

            migrationBuilder.CreateIndex(
                name: "IX_BusSchedules_RouteId",
                table: "BusSchedules",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_BusSchedules_BusScheduleId",
                table: "Tickets",
                column: "BusScheduleId",
                principalTable: "BusSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_BusSchedules_BusScheduleId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "BusSchedules");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_BusScheduleId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_MobileNumber",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "BusScheduleId",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "DroppingPoint",
                table: "Tickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "BoardingPoint",
                table: "Tickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "StopName",
                table: "RouteStops",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "BusNumber",
                table: "Buses",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);
        }
    }
}
