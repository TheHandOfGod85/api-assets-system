using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Departments_DepartmentName",
                table: "Assets");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Departments_DepartmentName",
                table: "Assets",
                column: "DepartmentName",
                principalTable: "Departments",
                principalColumn: "Name",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Departments_DepartmentName",
                table: "Assets");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Departments_DepartmentName",
                table: "Assets",
                column: "DepartmentName",
                principalTable: "Departments",
                principalColumn: "Name");
        }
    }
}
