using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateworktypeworkstatusauthorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkTypeId",
                table: "WorkStatuses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Author",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurposeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    DeclaredScore = table.Column<float>(type: "real", nullable: false),
                    FinalScore = table.Column<float>(type: "real", nullable: false),
                    TempHours = table.Column<int>(type: "int", nullable: false),
                    FinalHours = table.Column<int>(type: "int", nullable: false),
                    IsNotMatch = table.Column<bool>(type: "bit", nullable: false),
                    IsScored = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Author_AuthorRoles_AuthorRoleId",
                        column: x => x.AuthorRoleId,
                        principalTable: "AuthorRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Author_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Author_Purposes_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "Purposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkStatuses_WorkTypeId",
                table: "WorkStatuses",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Author_AuthorRoleId",
                table: "Author",
                column: "AuthorRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Author_EmployeeId",
                table: "Author",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Author_PurposeId",
                table: "Author",
                column: "PurposeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkStatuses_WorkTypes_WorkTypeId",
                table: "WorkStatuses",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkStatuses_WorkTypes_WorkTypeId",
                table: "WorkStatuses");

            migrationBuilder.DropTable(
                name: "Author");

            migrationBuilder.DropIndex(
                name: "IX_WorkStatuses_WorkTypeId",
                table: "WorkStatuses");

            migrationBuilder.DropColumn(
                name: "WorkTypeId",
                table: "WorkStatuses");
        }
    }
}
