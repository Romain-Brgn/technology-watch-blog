using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnologyWatchBlog.Migrations
{
    /// <inheritdoc />
    public partial class AddCleanContentToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CleanContent",
                table: "Articles",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CleanContent",
                table: "Articles");
        }
    }
}
