using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAdminAPIconsuming.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentResponsetbclmn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "AssignmentResponses",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AssignmentResponses");
        }
    }
}
