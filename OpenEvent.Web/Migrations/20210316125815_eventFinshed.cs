﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
    public partial class eventFinshed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Finished",
                table: "Events",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Finished",
                table: "Events");
        }
    }
#pragma warning restore CS1591
}
