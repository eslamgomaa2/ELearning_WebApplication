using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Learning.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddLiveSessionsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LiveSessionId",
                table: "LiveSessionAttendees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveSessionAttendees_LiveSessionId",
                table: "LiveSessionAttendees",
                column: "LiveSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveSessionAttendees_LiveSessions_LiveSessionId",
                table: "LiveSessionAttendees",
                column: "LiveSessionId",
                principalTable: "LiveSessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveSessionAttendees_LiveSessions_LiveSessionId",
                table: "LiveSessionAttendees");

            migrationBuilder.DropIndex(
                name: "IX_LiveSessionAttendees_LiveSessionId",
                table: "LiveSessionAttendees");

            migrationBuilder.DropColumn(
                name: "LiveSessionId",
                table: "LiveSessionAttendees");
        }
    }
}
