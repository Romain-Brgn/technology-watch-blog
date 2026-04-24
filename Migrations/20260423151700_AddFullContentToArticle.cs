using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnologyWatchBlog.Migrations
{
    /// <inheritdoc />
    public partial class AddFullContentToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullContent",
                table: "Articles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasFullContent",
                table: "Articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullContent",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "HasFullContent",
                table: "Articles");
        }
    }
}
