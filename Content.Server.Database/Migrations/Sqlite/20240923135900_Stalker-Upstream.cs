using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class StalkerUpstream : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "stalker_stats",
                columns: table => new
                {
                    stalker_stats_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    login = table.Column<string>(type: "TEXT", nullable: false),
                    characteristic = table.Column<string>(type: "TEXT", nullable: false),
                    value = table.Column<float>(type: "REAL", nullable: false),
                    last_trained = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalker_stats", x => x.stalker_stats_id);
                });

            migrationBuilder.CreateTable(
                name: "stalkers",
                columns: table => new
                {
                    stalkers_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    login = table.Column<string>(type: "TEXT", nullable: false),
                    storage = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalkers", x => x.stalkers_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stalker_stats");

            migrationBuilder.DropTable(
                name: "stalkers");
        }
    }
}
