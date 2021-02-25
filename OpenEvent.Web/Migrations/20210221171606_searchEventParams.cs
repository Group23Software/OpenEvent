using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
    public partial class searchEventParams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Params",
                table: "SearchEvents",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Params",
                table: "SearchEvents");
        }
    }
}
