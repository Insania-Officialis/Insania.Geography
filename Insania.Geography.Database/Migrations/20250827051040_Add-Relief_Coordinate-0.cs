using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddRelief_Coordinate0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "u_reliefs_coordinates",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    relief_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор рельефа"),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи"),
                    center = table.Column<Point>(type: "geometry", nullable: false, comment: "Координаты точки центра сущности"),
                    area = table.Column<double>(type: "double precision", nullable: false, comment: "Площадь сущности"),
                    zoom = table.Column<int>(type: "integer", nullable: false, comment: "Коэффициент масштаба отображения сущности"),
                    coordinate_id = table.Column<long>(type: "bigint", nullable: true, comment: "Идентификатор координаты")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_u_reliefs_coordinates", x => x.id);
                    table.ForeignKey(
                        name: "FK_u_reliefs_coordinates_c_relief_relief_id",
                        column: x => x.relief_id,
                        principalSchema: "insania_geography",
                        principalTable: "c_relief",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_reliefs_coordinates_r_coordinates_coordinate_id",
                        column: x => x.coordinate_id,
                        principalSchema: "insania_geography",
                        principalTable: "r_coordinates",
                        principalColumn: "id");
                },
                comment: "Координаты рельефов");

            migrationBuilder.CreateIndex(
                name: "IX_u_reliefs_coordinates_coordinate_id_relief_id_date_deleted",
                schema: "insania_geography",
                table: "u_reliefs_coordinates",
                columns: new[] { "coordinate_id", "relief_id", "date_deleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_u_reliefs_coordinates_relief_id",
                schema: "insania_geography",
                table: "u_reliefs_coordinates",
                column: "relief_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "u_reliefs_coordinates",
                schema: "insania_geography");
        }
    }
}
