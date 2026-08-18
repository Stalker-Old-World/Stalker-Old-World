using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class StalkerENAddMessengerUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop old tables (clean migration — existing data is not migrated)
            migrationBuilder.DropTable(
                name: "stalker_messenger_contacts");

            migrationBuilder.DropTable(
                name: "stalker_messenger_ids");

            // Recreate with user_id columns
            migrationBuilder.CreateTable(
                name: "stalker_messenger_ids",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    character_name = table.Column<string>(type: "text", nullable: false),
                    messenger_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalker_messenger_ids", x => new { x.user_id, x.character_name });
                });

            migrationBuilder.CreateIndex(
                name: "IX_stalker_messenger_ids_messenger_id",
                table: "stalker_messenger_ids",
                column: "messenger_id",
                unique: true);

            migrationBuilder.CreateTable(
                name: "stalker_messenger_contacts",
                columns: table => new
                {
                    owner_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_character_name = table.Column<string>(type: "text", nullable: false),
                    contact_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contact_character_name = table.Column<string>(type: "text", nullable: false),
                    faction_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalker_messenger_contacts", x => new { x.owner_user_id, x.owner_character_name, x.contact_user_id, x.contact_character_name });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stalker_messenger_contacts");

            migrationBuilder.DropTable(
                name: "stalker_messenger_ids");

            // Recreate original tables
            migrationBuilder.CreateTable(
                name: "stalker_messenger_ids",
                columns: table => new
                {
                    character_name = table.Column<string>(type: "text", nullable: false),
                    messenger_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalker_messenger_ids", x => x.character_name);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stalker_messenger_ids_messenger_id",
                table: "stalker_messenger_ids",
                column: "messenger_id",
                unique: true);

            migrationBuilder.CreateTable(
                name: "stalker_messenger_contacts",
                columns: table => new
                {
                    owner_character_name = table.Column<string>(type: "text", nullable: false),
                    contact_character_name = table.Column<string>(type: "text", nullable: false),
                    faction_name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stalker_messenger_contacts", x => new { x.owner_character_name, x.contact_character_name });
                });
        }
    }
}
