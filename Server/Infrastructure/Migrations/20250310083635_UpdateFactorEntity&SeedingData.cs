using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFactorEntitySeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AuthorRoleId",
                table: "Factors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Factors_AuthorRoleId",
                table: "Factors",
                column: "AuthorRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factors_AuthorRoles_AuthorRoleId",
                table: "Factors",
                column: "AuthorRoleId",
                principalTable: "AuthorRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factors_AuthorRoles_AuthorRoleId",
                table: "Factors");

            migrationBuilder.DropIndex(
                name: "IX_Factors_AuthorRoleId",
                table: "Factors");

            migrationBuilder.DropColumn(
                name: "AuthorRoleId",
                table: "Factors");
        }
    }
}
