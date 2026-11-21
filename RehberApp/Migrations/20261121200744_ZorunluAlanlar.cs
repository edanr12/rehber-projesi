using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RehberApp.Migrations
{
    /// <inheritdoc />
    public partial class ZorunluAlanlar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kisiler_Birimler_BirimId",
                table: "Kisiler");

            migrationBuilder.AlterColumn<int>(
                name: "BirimId",
                table: "Kisiler",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Kisiler_Birimler_BirimId",
                table: "Kisiler",
                column: "BirimId",
                principalTable: "Birimler",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Kisiler_Birimler_BirimId",
                table: "Kisiler");

            migrationBuilder.AlterColumn<int>(
                name: "BirimId",
                table: "Kisiler",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Kisiler_Birimler_BirimId",
                table: "Kisiler",
                column: "BirimId",
                principalTable: "Birimler",
                principalColumn: "Id");
        }
    }
}
