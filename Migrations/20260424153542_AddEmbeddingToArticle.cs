using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechnologyWatchBlog.Migrations
{
    /// <inheritdoc />
    public partial class AddEmbeddingToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmbeddedAt",
                table: "Articles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Embedding",
                table: "Articles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmbeddedAt",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Embedding",
                table: "Articles");
        }
    }
}
