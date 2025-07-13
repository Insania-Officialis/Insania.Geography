using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddColors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                schema: "insania_geography",
                table: "c_coordinates_types",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BorderColor",
                schema: "insania_geography",
                table: "c_coordinates_types",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                schema: "insania_geography",
                table: "c_coordinates_types");

            migrationBuilder.DropColumn(
                name: "BorderColor",
                schema: "insania_geography",
                table: "c_coordinates_types");
        }
    }
}
