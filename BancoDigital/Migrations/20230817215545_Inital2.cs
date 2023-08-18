using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDigital.Migrations
{
    /// <inheritdoc />
    public partial class Inital2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NomeTitular",
                table: "Conta",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Conta",
                keyColumn: "NomeTitular",
                keyValue: null,
                column: "NomeTitular",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NomeTitular",
                table: "Conta",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
