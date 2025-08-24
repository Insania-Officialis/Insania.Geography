using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Insania.Geography.Database.Migrations
{
    /// <inheritdoc />
    public partial class EditStartId1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                type: "bigint",
                nullable: false,
                comment: "Первичный ключ таблицы",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Первичный ключ таблицы")
                .Annotation("Npgsql:IdentitySequenceOptions", "'79', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'41', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "insania_geography",
                table: "r_coordinates",
                type: "bigint",
                nullable: false,
                comment: "Первичный ключ таблицы",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Первичный ключ таблицы")
                .Annotation("Npgsql:IdentitySequenceOptions", "'41', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'79', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "insania_geography",
                table: "u_geography_objects_coordinates",
                type: "bigint",
                nullable: false,
                comment: "Первичный ключ таблицы",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Первичный ключ таблицы")
                .Annotation("Npgsql:IdentitySequenceOptions", "'41', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'79', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                schema: "insania_geography",
                table: "r_coordinates",
                type: "bigint",
                nullable: false,
                comment: "Первичный ключ таблицы",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Первичный ключ таблицы")
                .Annotation("Npgsql:IdentitySequenceOptions", "'79', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'41', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
