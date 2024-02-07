using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lewBlazorServer.Migrations
{
    /// <inheritdoc />
    public partial class AddedOnStudy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnStudies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    ShowAfter = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lvl = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnStudies_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OnStudies_WordId",
                table: "OnStudies",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnStudies");
        }
    }
}
