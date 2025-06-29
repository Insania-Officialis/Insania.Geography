using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Geography.Database.Migrations.LogsApiGeography
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insania_logs_api_geography");

            migrationBuilder.CreateTable(
                name: "r_logs_api_geography",
                schema: "insania_logs_api_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи"),
                    method = table.Column<string>(type: "text", nullable: false, comment: "Наименование вызываемого метода"),
                    type = table.Column<string>(type: "text", nullable: false, comment: "Тип вызываемого метода"),
                    success = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак успешного выполнения"),
                    date_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата начала"),
                    date_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата окончания"),
                    data_in = table.Column<string>(type: "jsonb", nullable: true, comment: "Данные на вход"),
                    data_out = table.Column<string>(type: "jsonb", nullable: true, comment: "Данные на выход")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_logs_api_geography", x => x.id);
                },
                comment: "Логи сервиса географии");

            migrationBuilder.CreateIndex(
                name: "IX_r_logs_api_geography_data_in",
                schema: "insania_logs_api_geography",
                table: "r_logs_api_geography",
                column: "data_in")
                .Annotation("Npgsql:IndexMethod", "gin");

            migrationBuilder.CreateIndex(
                name: "IX_r_logs_api_geography_data_out",
                schema: "insania_logs_api_geography",
                table: "r_logs_api_geography",
                column: "data_out")
                .Annotation("Npgsql:IndexMethod", "gin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "r_logs_api_geography",
                schema: "insania_logs_api_geography");
        }
    }
}
