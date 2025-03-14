using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreshMigration_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Fields_SCImagoFieldId",
                table: "Authors");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_SCImagoFields_SCImagoFieldId",
                table: "Authors",
                column: "SCImagoFieldId",
                principalTable: "SCImagoFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_SCImagoFields_SCImagoFieldId",
                table: "Authors");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Fields_SCImagoFieldId",
                table: "Authors",
                column: "SCImagoFieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
