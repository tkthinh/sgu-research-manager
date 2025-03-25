using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorRoleHDSVNCKH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AuthorRoles",
                columns: new[] { "Id", "CreatedDate", "IsMainAuthor", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[] { new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "GV hướng dẫn", new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") });

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("166988df-84b4-4b0f-a1e0-8d356a1f4346"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("1ebfedcf-12c6-408a-82fd-170f9211d0d3"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("41d45d0a-39ea-417f-ba73-888b495525de"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("5aeed66d-b2ae-448f-8e30-f7c005c54ff2"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("60772df7-9150-4219-9ee8-ce5439144b0c"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("64780798-ffaf-48eb-be29-8a61fc4854a2"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("aee0c04a-26bc-4ef6-92b7-3d78f6ccaa61"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b986e0d6-36c8-4b73-ab80-566d519bff16"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("dd1ea2c7-4cc5-442d-b8fa-c6a4f8a663a2"),
                column: "AuthorRoleId",
                value: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("166988df-84b4-4b0f-a1e0-8d356a1f4346"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("1ebfedcf-12c6-408a-82fd-170f9211d0d3"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("41d45d0a-39ea-417f-ba73-888b495525de"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("5aeed66d-b2ae-448f-8e30-f7c005c54ff2"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("60772df7-9150-4219-9ee8-ce5439144b0c"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("64780798-ffaf-48eb-be29-8a61fc4854a2"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("aee0c04a-26bc-4ef6-92b7-3d78f6ccaa61"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b986e0d6-36c8-4b73-ab80-566d519bff16"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("dd1ea2c7-4cc5-442d-b8fa-c6a4f8a663a2"),
                column: "AuthorRoleId",
                value: null);
        }
    }
}
