using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassRoom.Migrations
{
    /// <inheritdoc />
    public partial class AddSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "StudentCourse",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourse_SessionId",
                table: "StudentCourse",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Session_SessionId",
                table: "StudentCourse",
                column: "SessionId",
                principalTable: "Session",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Session_SessionId",
                table: "StudentCourse");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourse_SessionId",
                table: "StudentCourse");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "StudentCourse");
        }
    }
}
