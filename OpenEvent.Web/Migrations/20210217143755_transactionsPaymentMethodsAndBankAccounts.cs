using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
#pragma warning disable CS1591
    public partial class transactionsPaymentMethodsAndBankAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    StripeBankAccountId = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true),
                    Currency = table.Column<string>(type: "longtext", nullable: true),
                    LastFour = table.Column<string>(type: "longtext", nullable: true),
                    Bank = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.StripeBankAccountId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    StripeCardId = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Name = table.Column<string>(type: "longtext", nullable: true),
                    Brand = table.Column<string>(type: "longtext", nullable: true),
                    Funding = table.Column<string>(type: "longtext", nullable: true),
                    ExpiryMonth = table.Column<int>(type: "int", nullable: false),
                    ExpiryYear = table.Column<int>(type: "int", nullable: false),
                    LastFour = table.Column<string>(type: "longtext", nullable: true),
                    Country = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.StripeCardId);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // migrationBuilder.CreateTable(
            //     name: "Transactions",
            //     columns: table => new
            //     {
            //         Id = table.Column<Guid>(type: "char(36)", nullable: false),
            //         TimeOfTransaction = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //         Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
            //         Paid = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //         StripeChargeId = table.Column<string>(type: "longtext", nullable: true),
            //         UserId = table.Column<Guid>(type: "char(36)", nullable: true),
            //         TicketId = table.Column<Guid>(type: "char(36)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Transactions", x => x.Id);
            //         table.ForeignKey(
            //             name: "FK_Transactions_Tickets_TicketId",
            //             column: x => x.TicketId,
            //             principalTable: "Tickets",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade);
            //         table.ForeignKey(
            //             name: "FK_Transactions_Users_UserId",
            //             column: x => x.UserId,
            //             principalTable: "Users",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Restrict);
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_UserId",
                table: "PaymentMethods",
                column: "UserId");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Transactions_TicketId",
            //     table: "Transactions",
            //     column: "TicketId",
            //     unique: true);
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_Transactions_UserId",
            //     table: "Transactions",
            //     column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            // migrationBuilder.DropTable(
            //     name: "Transactions");
        }
    }
#pragma warning restore CS1591
}
