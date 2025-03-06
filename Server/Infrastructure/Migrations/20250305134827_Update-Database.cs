using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkStatuses_WorkStatusId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "WorkStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Works_WorkStatusId",
                table: "Works");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkTypeId",
                table: "Purposes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Purposes_WorkTypeId",
                table: "Purposes",
                column: "WorkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purposes_WorkTypes_WorkTypeId",
                table: "Purposes",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purposes_WorkTypes_WorkTypeId",
                table: "Purposes");

            migrationBuilder.DropIndex(
                name: "IX_Purposes_WorkTypeId",
                table: "Purposes");

            migrationBuilder.DropColumn(
                name: "WorkTypeId",
                table: "Purposes");

            migrationBuilder.CreateTable(
                name: "WorkStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkStatuses_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkStatusId",
                table: "Works",
                column: "WorkStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkStatuses_WorkTypeId",
                table: "WorkStatuses",
                column: "WorkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkStatuses_WorkStatusId",
                table: "Works",
                column: "WorkStatusId",
                principalTable: "WorkStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
