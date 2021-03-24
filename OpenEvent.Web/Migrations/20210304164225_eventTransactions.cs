using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
    public partial class eventTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EventId",
                table: "Transactions",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_EventId",
                table: "Transactions",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Events_EventId",
                table: "Transactions",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Events_EventId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_EventId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Transactions");
        }
    }
#pragma warning restore CS1591
}
