using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreMvcAPI.Migrations
{
    public partial class _20200109 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoreMvcAPI.Data.IDataContext.Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreMvcAPI.Data.IDataContext.Customers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoreMvcAPI.Data.IDataContext.Customers");
        }
    }
}
