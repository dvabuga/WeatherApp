using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherApp.Migrations
{
    public partial class ModuleCreatedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Modules",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Modules");
        }
    }
}
