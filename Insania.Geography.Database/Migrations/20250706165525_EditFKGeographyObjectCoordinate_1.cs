using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class EditFKGeographyObjectCoordinate_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_u_geography_objects_coordinates_r_coordinates_coordinate_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_deleted",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                type: "timestamp without time zone",
                nullable: true,
                comment: "Дата удаления",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldComment: "Дата удаления");

            migrationBuilder.AlterColumn<long>(
                name: "coordinate_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                type: "bigint",
                nullable: true,
                comment: "Идентификатор координаты",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Идентификатор координаты");

            migrationBuilder.CreateIndex(
                name: "IX_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                columns: new[] { "coordinate_id", "geography_object_id", "date_deleted" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_u_geography_objects_coordinates_r_coordinates_coordinate_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                column: "coordinate_id",
                principalSchema: "insania_geography",
                principalTable: "r_coordinates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_u_geography_objects_coordinates_r_coordinates_coordinate_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates");

            migrationBuilder.DropIndex(
                name: "IX_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_deleted",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "Дата удаления",
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true,
                oldComment: "Дата удаления");

            migrationBuilder.AlterColumn<long>(
                name: "coordinate_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "Идентификатор координаты",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "Идентификатор координаты");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                columns: new[] { "coordinate_id", "geography_object_id", "date_deleted" });

            migrationBuilder.AddForeignKey(
                name: "FK_u_geography_objects_coordinates_r_coordinates_coordinate_id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                column: "coordinate_id",
                principalSchema: "insania_geography",
                principalTable: "r_coordinates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
