using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkAuthorEntity_AddSCImagoFieldEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_ProofStatuses_WorkProofId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "ManagerWorkScore",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "WorkStatusId",
                table: "Works",
                newName: "SCImagoFieldId");

            migrationBuilder.RenameColumn(
                name: "WorkProofId",
                table: "Works",
                newName: "ProofStatusId");

            migrationBuilder.RenameColumn(
                name: "TotalHours",
                table: "Works",
                newName: "FinalWorkHour");

            migrationBuilder.RenameColumn(
                name: "MainAuthorCount",
                table: "Works",
                newName: "TotalMainAuthors");

            migrationBuilder.RenameIndex(
                name: "IX_Works_WorkProofId",
                table: "Works",
                newName: "IX_Works_ProofStatusId");

            migrationBuilder.RenameColumn(
                name: "Hours",
                table: "Factors",
                newName: "ConvertHour");

            migrationBuilder.RenameColumn(
                name: "FinalHours",
                table: "Authors",
                newName: "TempWorkHour");

            migrationBuilder.RenameColumn(
                name: "DeclaredHours",
                table: "Authors",
                newName: "TempAuthorHour");

            migrationBuilder.AddColumn<int>(
                name: "FinalAuthorHour",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SCImagoFields",
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
                    table.PrimaryKey("PK_SCImagoFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SCImagoFields_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_SCImagoFieldId",
                table: "Works",
                column: "SCImagoFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_SCImagoFields_WorkTypeId",
                table: "SCImagoFields",
                column: "WorkTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Works_ProofStatuses_ProofStatusId",
                table: "Works",
                column: "ProofStatusId",
                principalTable: "ProofStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_SCImagoFields_SCImagoFieldId",
                table: "Works",
                column: "SCImagoFieldId",
                principalTable: "SCImagoFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_ProofStatuses_ProofStatusId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_SCImagoFields_SCImagoFieldId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "SCImagoFields");

            migrationBuilder.DropIndex(
                name: "IX_Works_SCImagoFieldId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "FinalAuthorHour",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "TotalMainAuthors",
                table: "Works",
                newName: "MainAuthorCount");

            migrationBuilder.RenameColumn(
                name: "SCImagoFieldId",
                table: "Works",
                newName: "WorkStatusId");

            migrationBuilder.RenameColumn(
                name: "ProofStatusId",
                table: "Works",
                newName: "WorkProofId");

            migrationBuilder.RenameColumn(
                name: "FinalWorkHour",
                table: "Works",
                newName: "TotalHours");

            migrationBuilder.RenameIndex(
                name: "IX_Works_ProofStatusId",
                table: "Works",
                newName: "IX_Works_WorkProofId");

            migrationBuilder.RenameColumn(
                name: "ConvertHour",
                table: "Factors",
                newName: "Hours");

            migrationBuilder.RenameColumn(
                name: "TempWorkHour",
                table: "Authors",
                newName: "FinalHours");

            migrationBuilder.RenameColumn(
                name: "TempAuthorHour",
                table: "Authors",
                newName: "DeclaredHours");

            migrationBuilder.AddColumn<float>(
                name: "ManagerWorkScore",
                table: "Works",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FinalScore",
                table: "Authors",
                type: "real",
                nullable: false,
                defaultValue: 0f);

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
