using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnologyWatchBlog.Migrations
{
    /// <inheritdoc />
    public partial class RefactorArticleForDevTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrichmentAttempted",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "HasFullContent",
                table: "Articles");

            migrationBuilder.RenameColumn(
                name: "FullContent",
                table: "Articles",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "EnrichmentError",
                table: "Articles",
                newName: "Author");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "Articles",
                newName: "FullContent");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Articles",
                newName: "EnrichmentError");

            migrationBuilder.AddColumn<bool>(
                name: "EnrichmentAttempted",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFullContent",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
