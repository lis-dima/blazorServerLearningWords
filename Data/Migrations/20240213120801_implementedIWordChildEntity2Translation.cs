using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lewBlazorServer.Migrations
{
    /// <inheritdoc />
    public partial class implementedIWordChildEntity2Translation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WordChildType",
                table: "Translations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordChildType",
                table: "Translations");
        }
    }
}
