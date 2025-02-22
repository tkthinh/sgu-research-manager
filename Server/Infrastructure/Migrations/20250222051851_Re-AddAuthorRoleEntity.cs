using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReAddAuthorRoleEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRoles_WorkTypes_TypeId",
                table: "AuthorRoles");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "AuthorRoles",
                newName: "WorkTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorRoles_TypeId",
                table: "AuthorRoles",
                newName: "IX_AuthorRoles_WorkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRoles_WorkTypes_WorkTypeId",
                table: "AuthorRoles",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorRoles_WorkTypes_WorkTypeId",
                table: "AuthorRoles");

            migrationBuilder.RenameColumn(
                name: "WorkTypeId",
                table: "AuthorRoles",
                newName: "TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorRoles_WorkTypeId",
                table: "AuthorRoles",
                newName: "IX_AuthorRoles_TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorRoles_WorkTypes_TypeId",
                table: "AuthorRoles",
                column: "TypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
