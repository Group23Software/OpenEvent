using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
    public partial class userAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine1",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_AddressLine2",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_City",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_CountryCode",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_CountryName",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Lat",
                table: "Users",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Address_Lon",
                table: "Users",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "Users",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_AddressLine1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_AddressLine2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_CountryCode",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_CountryName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Lat",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_Lon",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "Users");
        }
    }
}
