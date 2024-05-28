using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Departments_DepartmentName",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_DepartmentName",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Assets");

            migrationBuilder.AddColumn<Guid>(
                name: "AssetId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_AssetId",
                table: "Departments",
                column: "AssetId",
                unique: true,
                filter: "[AssetId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Assets_AssetId",
                table: "Departments",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Assets_AssetId",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_AssetId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Departments");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Assets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_DepartmentName",
                table: "Assets",
                column: "DepartmentName");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Departments_DepartmentName",
                table: "Assets",
                column: "DepartmentName",
                principalTable: "Departments",
                principalColumn: "Name");
        }
    }
}
