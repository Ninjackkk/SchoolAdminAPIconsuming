using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAdminAPIconsuming.Migrations
{
    /// <inheritdoc />
    public partial class assignmentchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_STDs_StdId",
                table: "Assignments");

            migrationBuilder.AlterColumn<int>(
                name: "StdId",
                table: "Assignments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "StdName",
                table: "Assignments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_STDs_StdId",
                table: "Assignments",
                column: "StdId",
                principalTable: "STDs",
                principalColumn: "StdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_STDs_StdId",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "StdName",
                table: "Assignments");

            migrationBuilder.AlterColumn<int>(
                name: "StdId",
                table: "Assignments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_STDs_StdId",
                table: "Assignments",
                column: "StdId",
                principalTable: "STDs",
                principalColumn: "StdId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
