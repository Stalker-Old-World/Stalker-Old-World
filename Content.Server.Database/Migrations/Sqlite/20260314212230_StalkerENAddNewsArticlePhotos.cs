using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class StalkerENAddNewsArticlePhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "photo_id",
                table: "stalker_news_articles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "stalker_news_article_photos",
                columns: table => new
                {
                    stalker_news_article_photos_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    photo_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    photo_data = table.Column<byte[]>(type: "BLOB", nullable: false),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalker_news_article_photos", x => x.stalker_news_article_photos_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stalker_news_article_photos_photo_id",
                table: "stalker_news_article_photos",
                column: "photo_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stalker_news_article_photos");

            migrationBuilder.DropColumn(
                name: "photo_id",
                table: "stalker_news_articles");
        }
    }
}
