using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "insania_geography");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "c_coordinates_types",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeDiscriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
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
                    table.PrimaryKey("PK_c_coordinates_types", x => x.id);
                    table.UniqueConstraint("AK_c_coordinates_types_alias", x => x.alias);
                },
                comment: "Типы координат географии");

            migrationBuilder.CreateTable(
                name: "c_geography_objects_types",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_c_geography_objects_types", x => x.id);
                    table.UniqueConstraint("AK_c_geography_objects_types_alias", x => x.alias);
                },
                comment: "Типы географических объектов");

            migrationBuilder.CreateTable(
                name: "r_coordinates",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    polygon = table.Column<Polygon>(type: "geometry", nullable: false, comment: "Полигон (массив координат)"),
                    type_id = table.Column<long>(type: "bigint", nullable: true, comment: "Идентификатор типа координаты"),
                    TypeDiscriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    date_create = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата создания"),
                    username_create = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, создавшего"),
                    date_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, comment: "Дата обновления"),
                    username_update = table.Column<string>(type: "text", nullable: false, comment: "Логин пользователя, обновившего"),
                    date_deleted = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, comment: "Дата удаления"),
                    is_system = table.Column<bool>(type: "boolean", nullable: false, comment: "Признак системной записи")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_r_coordinates", x => x.id);
                    table.ForeignKey(
                        name: "FK_r_coordinates_c_coordinates_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "insania_geography",
                        principalTable: "c_coordinates_types",
                        principalColumn: "id");
                },
                comment: "Координаты географии");

            migrationBuilder.CreateTable(
                name: "c_geography_objects",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор типа"),
                    parent_id = table.Column<long>(type: "bigint", nullable: true, comment: "Идентификатор родителя"),
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
                    table.PrimaryKey("PK_c_geography_objects", x => x.id);
                    table.UniqueConstraint("AK_c_geography_objects_alias", x => x.alias);
                    table.ForeignKey(
                        name: "FK_c_geography_objects_c_geography_objects_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "insania_geography",
                        principalTable: "c_geography_objects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_c_geography_objects_c_geography_objects_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "insania_geography",
                        principalTable: "c_geography_objects_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Географические объекты");

            migrationBuilder.CreateTable(
                name: "u_geography_objects_coordinates",
                schema: "insania_geography",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Первичный ключ таблицы")
                        .Annotation("Npgsql:IdentitySequenceOptions", "'4', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    geography_object_id = table.Column<long>(type: "bigint", nullable: false, comment: "Идентификатор географического объекта"),
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
                    table.PrimaryKey("PK_u_geography_objects_coordinates", x => x.id);
                    table.ForeignKey(
                        name: "FK_u_geography_objects_coordinates_c_geography_objects_geograp~",
                        column: x => x.geography_object_id,
                        principalSchema: "insania_geography",
                        principalTable: "c_geography_objects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_u_geography_objects_coordinates_r_coordinates_coordinate_id",
                        column: x => x.coordinate_id,
                        principalSchema: "insania_geography",
                        principalTable: "r_coordinates",
                        principalColumn: "id");
                },
                comment: "Координаты географических объектов");

            migrationBuilder.CreateIndex(
                name: "IX_c_geography_objects_parent_id",
                schema: "insania_geography",
                table: "c_geography_objects",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_c_geography_objects_type_id",
                schema: "insania_geography",
                table: "c_geography_objects",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_r_coordinates_polygon",
                schema: "insania_geography",
                table: "r_coordinates",
                column: "polygon")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_r_coordinates_type_id",
                schema: "insania_geography",
                table: "r_coordinates",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                columns: new[] { "coordinate_id", "geography_object_id", "date_deleted" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_u_geography_objects_coordinates_geography_object_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                column: "geography_object_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "u_geography_objects_coordinates",
                schema: "insania_geography");

            migrationBuilder.DropTable(
                name: "c_geography_objects",
                schema: "insania_geography");

            migrationBuilder.DropTable(
                name: "r_coordinates",
                schema: "insania_geography");

            migrationBuilder.DropTable(
                name: "c_geography_objects_types",
                schema: "insania_geography");

            migrationBuilder.DropTable(
                name: "c_coordinates_types",
                schema: "insania_geography");
        }
    }
}
