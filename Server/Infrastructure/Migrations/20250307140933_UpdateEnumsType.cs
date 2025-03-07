using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnumsType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AcademicRanks_AcademicRankId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_OfficerRanks_OfficerRankId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_ProofStatuses_WorkProofId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "AcademicRanks");

            migrationBuilder.DropTable(
                name: "OfficerRanks");

            migrationBuilder.DropTable(
                name: "ProofStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Works_WorkProofId",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Employees_AcademicRankId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_OfficerRankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "WorkProofId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "AcademicRankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OfficerRankId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "ProofStatus",
                table: "Works",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AcademicTitle",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficerRank",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProofStatus",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "AcademicTitle",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OfficerRank",
                table: "Employees");

            migrationBuilder.AddColumn<Guid>(
                name: "WorkProofId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AcademicRankId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OfficerRankId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AcademicRanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicRanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficerRanks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficerRanks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProofStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProofStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkProofId",
                table: "Works",
                column: "WorkProofId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_AcademicRankId",
                table: "Employees",
                column: "AcademicRankId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_OfficerRankId",
                table: "Employees",
                column: "OfficerRankId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AcademicRanks_AcademicRankId",
                table: "Employees",
                column: "AcademicRankId",
                principalTable: "AcademicRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_OfficerRanks_OfficerRankId",
                table: "Employees",
                column: "OfficerRankId",
                principalTable: "OfficerRanks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_ProofStatuses_WorkProofId",
                table: "Works",
                column: "WorkProofId",
                principalTable: "ProofStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
