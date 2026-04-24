using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnologyWatchBlog.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrichmentTrackingToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EnrichmentAttempted",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EnrichmentError",
                table: "Articles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnrichmentAttempted",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "EnrichmentError",
                table: "Articles");
        }
    }
}
