using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
    public partial class promoDiscountToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "Promos",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Promos",
                type: "double",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
