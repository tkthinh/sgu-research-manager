using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkAuthors",
                table: "WorkAuthors");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Works",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProofStatus",
                table: "Authors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkAuthors",
                table: "WorkAuthors",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Works_ExchangeDeadline",
                table: "Works",
                column: "ExchangeDeadline");

            migrationBuilder.CreateIndex(
                name: "IX_Works_Source",
                table: "Works",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAuthors_UserId_WorkId",
                table: "WorkAuthors",
                columns: new[] { "UserId", "WorkId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkAuthors_WorkId",
                table: "WorkAuthors",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_ProofStatus",
                table: "Authors",
                column: "ProofStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_WorkId",
                table: "Authors",
                column: "WorkId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRegistrations_AcademicYearId",
                table: "AuthorRegistrations",
                column: "AcademicYearId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Works_ExchangeDeadline",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_Source",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkAuthors",
                table: "WorkAuthors");

            migrationBuilder.DropIndex(
                name: "IX_WorkAuthors_UserId_WorkId",
                table: "WorkAuthors");

            migrationBuilder.DropIndex(
                name: "IX_WorkAuthors_WorkId",
                table: "WorkAuthors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_ProofStatus",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_WorkId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_AuthorRegistrations_AcademicYearId",
                table: "AuthorRegistrations");

            migrationBuilder.AlterColumn<string>(
                name: "Source",
                table: "Works",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProofStatus",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkAuthors",
                table: "WorkAuthors",
                columns: new[] { "WorkId", "UserId" });
        }
    }
}
