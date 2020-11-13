using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherApp.Migrations
{
    public partial class UpdateModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Version",
                table: "Modules",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<byte[]>(
                name: "Assembly",
                table: "Modules",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assembly",
                table: "Modules");

            migrationBuilder.AlterColumn<int>(
                name: "Version",
                table: "Modules",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
