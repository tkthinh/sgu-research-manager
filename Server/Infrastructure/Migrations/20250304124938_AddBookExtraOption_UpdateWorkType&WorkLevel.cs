using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBookExtraOption_UpdateWorkTypeWorkLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasExtraOption",
                table: "WorkTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkTypeId",
                table: "WorkLevels",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BookExtraOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookExtraOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookExtraOptions_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkLevels_WorkTypeId",
                table: "WorkLevels",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BookExtraOptions_WorkTypeId",
                table: "BookExtraOptions",
                column: "WorkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLevels_WorkTypes_WorkTypeId",
                table: "WorkLevels",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLevels_WorkTypes_WorkTypeId",
                table: "WorkLevels");

            migrationBuilder.DropTable(
                name: "BookExtraOptions");

            migrationBuilder.DropIndex(
                name: "IX_WorkLevels_WorkTypeId",
                table: "WorkLevels");

            migrationBuilder.DropColumn(
                name: "HasExtraOption",
                table: "WorkTypes");

            migrationBuilder.DropColumn(
                name: "WorkTypeId",
                table: "WorkLevels");
        }
    }
}
