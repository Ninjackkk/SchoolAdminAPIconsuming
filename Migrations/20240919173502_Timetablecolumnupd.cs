using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAdminAPIconsuming.Migrations
{
    /// <inheritdoc />
    public partial class Timetablecolumnupd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_STDs_StdId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_StdId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "StdId",
                table: "Timetables");

            migrationBuilder.AddColumn<string>(
                name: "STD",
                table: "Timetables",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STD",
                table: "Timetables");

            migrationBuilder.AddColumn<int>(
                name: "StdId",
                table: "Timetables",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_StdId",
                table: "Timetables",
                column: "StdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_STDs_StdId",
                table: "Timetables",
                column: "StdId",
                principalTable: "STDs",
                principalColumn: "StdId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
