using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
    public partial class isCanceled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isCanceled",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isCanceled",
                table: "Events");
        }
    }
}
