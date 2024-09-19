using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAdminAPIconsuming.Migrations
{
    /// <inheritdoc />
    public partial class AssignmentResponsetbclmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Score",
                table: "AssignmentResponses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "AssignmentResponses");
        }
    }
}
