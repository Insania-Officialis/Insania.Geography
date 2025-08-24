using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddRelief0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "c_relief",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    level = table.Column<int>(type: "integer", nullable: false, comment: "Уровень"),
                    color = table.Column<string>(type: "text", nullable: false, comment: "Цвет на карте"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    name = table.Column<string>(type: "text", nullable: false, comment: "Наименование"),
                    alias = table.Column<string>(type: "text", nullable: false, comment: "Псевдоним")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_c_relief", x => x.id);
                    table.UniqueConstraint("AK_c_relief_alias", x => x.alias);
                },
                comment: "Рельефы");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "c_relief",
                schema: "insania_geography");
        }
    }
}
