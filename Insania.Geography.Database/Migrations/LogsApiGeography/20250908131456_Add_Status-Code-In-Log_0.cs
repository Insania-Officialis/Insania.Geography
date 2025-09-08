using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insania.Geography.Database.Migrations.LogsApiGeography
{
    /// <inheritdoc />
    public partial class Add_StatusCodeInLog_0 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "status_code",
                schema: "insania_logs_api_geography",
                table: "r_logs_api_geography",
                type: "integer",
                nullable: true,
                comment: "Код статуса");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status_code",
                schema: "insania_logs_api_geography",
                table: "r_logs_api_geography");
        }
    }
}
