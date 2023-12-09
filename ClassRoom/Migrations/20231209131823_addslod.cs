using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.Migrations
{
    /// <inheritdoc />
    public partial class addslod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Finish",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Bookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "Session_End_Date",
                table: "Session",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Session_Start_Date",
                table: "Session",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "LecturerCourses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlodId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Slods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slods", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LecturerCourses_SessionId",
                table: "LecturerCourses",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SessionId",
                table: "Bookings",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SlodId",
                table: "Bookings",
                column: "SlodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Session_SessionId",
                table: "Bookings",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Slods_SlodId",
                table: "Bookings",
                column: "SlodId",
                principalTable: "Slods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LecturerCourses_Session_SessionId",
                table: "LecturerCourses",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Session_SessionId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Slods_SlodId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_LecturerCourses_Session_SessionId",
                table: "LecturerCourses");

            migrationBuilder.DropTable(
                name: "Slods");

            migrationBuilder.DropIndex(
                name: "IX_LecturerCourses_SessionId",
                table: "LecturerCourses");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SessionId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SlodId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Session_End_Date",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "Session_Start_Date",
                table: "Session");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "LecturerCourses");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "SlodId",
                table: "Bookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Finish",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
