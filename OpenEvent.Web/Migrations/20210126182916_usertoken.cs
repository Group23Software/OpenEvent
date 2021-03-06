﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
    public partial class usertoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Users",
                type: "longtext",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Users");
        }
    }
#pragma warning restore CS1591
}
