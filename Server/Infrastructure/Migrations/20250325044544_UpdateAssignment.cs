using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorRoleId",
                table: "Authors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("12fcd9aa-301b-42f0-9195-d1ca60011613"),
                column: "Name",
                value: "Giải pháp hữu ích");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("568d791f-0a4f-4097-b7e2-9ed9836c3131"),
                column: "Name",
                value: "Giải pháp hữu ích");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("99f88266-0281-49b1-bf44-397cf86816d5"),
                column: "Name",
                value: "Giải pháp hữu ích");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("9ae95c37-54eb-415d-939a-d70008a33b28"),
                column: "Name",
                value: "Giải pháp hữu ích");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorRoleId",
                table: "Authors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("12fcd9aa-301b-42f0-9195-d1ca60011613"),
                column: "Name",
                value: "Tác phẩm nghệ thuật");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("568d791f-0a4f-4097-b7e2-9ed9836c3131"),
                column: "Name",
                value: "Tác phẩm nghệ thuật");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("99f88266-0281-49b1-bf44-397cf86816d5"),
                column: "Name",
                value: "Tác phẩm nghệ thuật");

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("9ae95c37-54eb-415d-939a-d70008a33b28"),
                column: "Name",
                value: "Tác phẩm nghệ thuật");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignments_Users_UserId",
                table: "Assignments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
