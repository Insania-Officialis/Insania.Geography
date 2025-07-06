using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class EditFKGeographyObjectCoordinate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_u_geography_objects_coordinates_coordinate_id_geography_obj~",
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

            migrationBuilder.AddUniqueConstraint(
                name: "AK_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                columns: new[] { "coordinate_id", "geography_object_id", "date_deleted" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddUniqueConstraint(
                name: "AK_u_geography_objects_coordinates_coordinate_id_geography_obj~",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                columns: new[] { "coordinate_id", "geography_object_id" });
        }
    }
}
