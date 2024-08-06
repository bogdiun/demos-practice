using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesService.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Notes");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "Notes",
                newSchema: "Notes");

            migrationBuilder.RenameTable(
                name: "MediaTypes",
                newName: "MediaTypes",
                newSchema: "Notes");

            migrationBuilder.RenameTable(
                name: "CategoryNote",
                newName: "CategoryNote",
                newSchema: "Notes");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "Notes");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "Notes",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "Notes",
                table: "Notes");

            migrationBuilder.RenameTable(
                name: "Notes",
                schema: "Notes",
                newName: "Notes");

            migrationBuilder.RenameTable(
                name: "MediaTypes",
                schema: "Notes",
                newName: "MediaTypes");

            migrationBuilder.RenameTable(
                name: "CategoryNote",
                schema: "Notes",
                newName: "CategoryNote");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Notes",
                newName: "Categories");
        }
    }
}
