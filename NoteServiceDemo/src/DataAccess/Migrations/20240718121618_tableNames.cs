using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesService.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class tableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryNote_Category_CategoriesId",
                table: "CategoryNote");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_MediaType_MediaTypeId",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaType",
                table: "MediaType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "MediaType",
                newName: "MediaTypes");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaTypes",
                table: "MediaTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryNote_Categories_CategoriesId",
                table: "CategoryNote",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_MediaTypes_MediaTypeId",
                table: "Notes",
                column: "MediaTypeId",
                principalTable: "MediaTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryNote_Categories_CategoriesId",
                table: "CategoryNote");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_MediaTypes_MediaTypeId",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaTypes",
                table: "MediaTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "MediaTypes",
                newName: "MediaType");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaType",
                table: "MediaType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryNote_Category_CategoriesId",
                table: "CategoryNote",
                column: "CategoriesId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_MediaType_MediaTypeId",
                table: "Notes",
                column: "MediaTypeId",
                principalTable: "MediaType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
