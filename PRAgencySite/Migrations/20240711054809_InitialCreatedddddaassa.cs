using Microsoft.EntityFrameworkCore.Migrations;

namespace PRAgencySite.Migrations
{
    public partial class InitialCreatedddddaassa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Niche",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Niche",
                table: "Brands");
        }
    }
}
