using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class ban_notify_trigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing trigger and function if they exist
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS notify_on_server_ban_insert ON server_ban;
                DROP FUNCTION IF EXISTS send_server_ban_notification();
            """);

            // Recreate the function
            migrationBuilder.Sql("""
                CREATE OR REPLACE FUNCTION send_server_ban_notification()
                    RETURNS trigger AS $$
                    DECLARE
                        x_server_id INTEGER;
                    BEGIN
                        SELECT round.server_id INTO x_server_id FROM round WHERE round.round_id = NEW.round_id;

                        PERFORM pg_notify('ban_notification', json_build_object('ban_id', NEW.server_ban_id, 'server_id', x_server_id)::text);
                        RETURN NEW;
                    END;
                    $$ LANGUAGE plpgsql;
            """);

            // Recreate the trigger
            migrationBuilder.Sql("""
                CREATE TRIGGER notify_on_server_ban_insert
                    AFTER INSERT ON server_ban
                    FOR EACH ROW
                    EXECUTE FUNCTION send_server_ban_notification();
            """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the trigger and function
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS notify_on_server_ban_insert ON server_ban;
                DROP FUNCTION IF EXISTS send_server_ban_notification();
            """);
        }
    }
}

