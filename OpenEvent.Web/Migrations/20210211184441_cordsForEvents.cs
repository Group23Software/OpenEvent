using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
    public partial class cordsForEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Address_Lat",
                table: "Events",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Lon",
                table: "Events",
                type: "double",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_Lat",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Address_Lon",
                table: "Events");
        }
    }
#pragma warning restore CS1591
}
