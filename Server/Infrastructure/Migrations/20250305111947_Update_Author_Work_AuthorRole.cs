using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Author_Work_AuthorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_ProofStatuses_ProofId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkLevels_LevelId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkStatuses_StatusId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkTypes_TypeId",
                table: "Works");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Works",
                newName: "WorkTypeId");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Works",
                newName: "WorkStatusId");

            migrationBuilder.RenameColumn(
                name: "ProofId",
                table: "Works",
                newName: "WorkProofId");

            migrationBuilder.RenameColumn(
                name: "LevelId",
                table: "Works",
                newName: "WorkLevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_TypeId",
                table: "Works",
                newName: "IX_Works_WorkTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_StatusId",
                table: "Works",
                newName: "IX_Works_WorkStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_ProofId",
                table: "Works",
                newName: "IX_Works_WorkProofId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_LevelId",
                table: "Works",
                newName: "IX_Works_WorkLevelId");

            migrationBuilder.AlterColumn<int>(
                name: "TotalAuthors",
                table: "Works",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MainAuthorCount",
                table: "Works",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Authors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CoAuthors",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_ProofStatuses_WorkProofId",
                table: "Works",
                column: "WorkProofId",
                principalTable: "ProofStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkLevels_WorkLevelId",
                table: "Works",
                column: "WorkLevelId",
                principalTable: "WorkLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkStatuses_WorkStatusId",
                table: "Works",
                column: "WorkStatusId",
                principalTable: "WorkStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkTypes_WorkTypeId",
                table: "Works",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Works_ProofStatuses_WorkProofId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkLevels_WorkLevelId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkStatuses_WorkStatusId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_WorkTypes_WorkTypeId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "CoAuthors",
                table: "Authors");

            migrationBuilder.RenameColumn(
                name: "WorkTypeId",
                table: "Works",
                newName: "TypeId");

            migrationBuilder.RenameColumn(
                name: "WorkStatusId",
                table: "Works",
                newName: "StatusId");

            migrationBuilder.RenameColumn(
                name: "WorkProofId",
                table: "Works",
                newName: "ProofId");

            migrationBuilder.RenameColumn(
                name: "WorkLevelId",
                table: "Works",
                newName: "LevelId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_WorkTypeId",
                table: "Works",
                newName: "IX_Works_TypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_WorkStatusId",
                table: "Works",
                newName: "IX_Works_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_WorkProofId",
                table: "Works",
                newName: "IX_Works_ProofId");

            migrationBuilder.RenameIndex(
                name: "IX_Works_WorkLevelId",
                table: "Works",
                newName: "IX_Works_LevelId");

            migrationBuilder.AlterColumn<int>(
                name: "TotalAuthors",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MainAuthorCount",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Position",
                table: "Authors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_ProofStatuses_ProofId",
                table: "Works",
                column: "ProofId",
                principalTable: "ProofStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkLevels_LevelId",
                table: "Works",
                column: "LevelId",
                principalTable: "WorkLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkStatuses_StatusId",
                table: "Works",
                column: "StatusId",
                principalTable: "WorkStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_WorkTypes_TypeId",
                table: "Works",
                column: "TypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
