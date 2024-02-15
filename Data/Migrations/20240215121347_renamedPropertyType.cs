using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lewBlazorServer.Migrations
{
    /// <inheritdoc />
    public partial class renamedPropertyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WordChildType",
                table: "Translations",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Translations",
                newName: "WordChildType");
        }
    }
}
