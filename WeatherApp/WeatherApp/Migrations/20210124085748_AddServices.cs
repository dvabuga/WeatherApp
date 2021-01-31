using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherApp.Migrations
{
    public partial class AddServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ServiceID = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    IsDefaultModule = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DateTimeCreate = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserServiceModules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ServiceId = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServiceModules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCities_CityId",
                table: "UserCities",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCities_Cities_CityId",
                table: "UserCities",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCities_Cities_CityId",
                table: "UserCities");

            migrationBuilder.DropTable(
                name: "ServiceModules");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "UserServiceModules");

            migrationBuilder.DropIndex(
                name: "IX_UserCities_CityId",
                table: "UserCities");
        }
    }
}
