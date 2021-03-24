using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
#pragma warning disable CS1591
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Events_EventId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Events_Images");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Events",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "money(65,30)");

            migrationBuilder.AddColumn<Guid>(
                name: "Thumbnail_Id",
                table: "Events",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail_Label",
                table: "Events",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Thumbnail_Source",
                table: "Events",
                type: "longblob",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events_Images",
                table: "Events_Images",
                columns: new[] { "EventId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Images_Events_EventId",
                table: "Events_Images",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Images_Events_EventId",
                table: "Events_Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events_Images",
                table: "Events_Images");

            migrationBuilder.DropColumn(
                name: "Thumbnail_Id",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Thumbnail_Label",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Thumbnail_Source",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events_Images",
                newName: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "longtext",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Events",
                type: "money(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                columns: new[] { "EventId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Events_EventId",
                table: "Image",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
#pragma warning restore CS1591
}
