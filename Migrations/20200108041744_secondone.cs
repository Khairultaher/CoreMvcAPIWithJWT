using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreMvcAPI.Migrations
{
    public partial class secondone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserRoll",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRoll",
                table: "AspNetUsers");
        }
    }
}
