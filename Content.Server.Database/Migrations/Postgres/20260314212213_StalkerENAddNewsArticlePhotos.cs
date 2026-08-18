using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
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
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "stalker_news_article_photos",
                columns: table => new
                {
                    stalker_news_article_photos_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    photo_id = table.Column<Guid>(type: "uuid", nullable: false),
                    photo_data = table.Column<byte[]>(type: "bytea", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
