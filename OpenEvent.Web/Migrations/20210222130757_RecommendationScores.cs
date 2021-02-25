using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OpenEvent.Web.Migrations
{
    public partial class RecommendationScores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecommendationScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CategoryId = table.Column<Guid>(type: "char(36)", nullable: true),
                    Weight = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationScores_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecommendationScores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationScores_CategoryId",
                table: "RecommendationScores",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationScores_UserId",
                table: "RecommendationScores",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendationScores");
        }
    }
}
