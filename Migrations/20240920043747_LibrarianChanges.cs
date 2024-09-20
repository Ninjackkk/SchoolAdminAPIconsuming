using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolAdminAPIconsuming.Migrations
{
    /// <inheritdoc />
    public partial class LibrarianChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Librarian_name",
                table: "Librarians",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Librarian_id",
                table: "Librarians",
                newName: "LibrarianId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Librarians",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "HireDate",
                table: "Librarians",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlySalary",
                table: "Librarians",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "HireDate",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "MonthlySalary",
                table: "Librarians");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Librarians",
                newName: "Librarian_name");

            migrationBuilder.RenameColumn(
                name: "LibrarianId",
                table: "Librarians",
                newName: "Librarian_id");
        }
    }
}
