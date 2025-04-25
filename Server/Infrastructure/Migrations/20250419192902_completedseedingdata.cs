using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class completedseedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("14b7a7e8-7327-450e-a5ca-f7d836b14499"),
                column: "AuthorRoleId",
                value: new Guid("11eea600-4495-486c-985d-57de08b8b5da"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b1131264-329f-4908-8e71-8b36088d3dde"),
                column: "AuthorRoleId",
                value: new Guid("5f65dffc-5e3a-46a8-9bc6-1bacce9ef3fa"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b74daf03-dc04-4738-ae87-97ec0faa07c1"),
                column: "AuthorRoleId",
                value: new Guid("d8d1af53-3354-4af3-a18f-85c6ee46e750"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("d3707663-2b44-4d95-93b7-37756d3e302c"),
                column: "AuthorRoleId",
                value: new Guid("6304f87f-439a-477d-b989-31df3b6e06b6"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("e2f96885-2bb7-4668-9c2f-6d6d313c09f7"),
                column: "AuthorRoleId",
                value: new Guid("be6b03da-7853-48ab-93b3-81da27c3271e"));

            migrationBuilder.InsertData(
                table: "Factors",
                columns: new[] { "Id", "AuthorRoleId", "ConvertHour", "CreatedDate", "MaxAllowed", "ModifiedDate", "Name", "PurposeId", "ScoreLevel", "WorkLevelId", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("05a85289-1c41-419f-b8c8-a680833a532d"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Trường được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("2971a6c0-9bfa-4c62-91db-e290a9008856"), new Guid("8bac0cd7-b553-42cc-af1a-5d50d32a6fac"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"), 23, null, new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("2c5a2f69-f5c6-493e-968b-3e844154ea0b"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Scopus được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("371136aa-c37c-4f80-88b9-916273b3484d"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Quốc gia được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("3be58f79-b3c8-4ae2-b761-1d020c064584"), new Guid("b0923868-3ce3-4653-97fa-d6925771ce64"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("3da2c117-b32f-4687-89b8-ba9544920f35"), 23, null, new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("53b25eec-2419-4cee-a256-c22767dc3a12"), new Guid("3dfd761c-256e-442f-99fb-136d27b4cea5"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"), 23, null, new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("5eb303f2-4a38-4ecb-a053-6fa2670186fb"), new Guid("3f0d8b5e-99da-4702-bc34-1b36c99cbdaa"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("494e049e-0972-4ff0-a786-6e00880955fc"), 23, null, new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("61502bfe-fba5-47d4-9aa9-c1ec01d7fb0f"), new Guid("4a85d698-7809-4912-923f-18c3f0a2e676"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"), 23, null, new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("65a1f78b-0de9-415e-91e9-40c03fb25275"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Quốc tế được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("83d5e873-4216-4e5e-969d-88cb7aa596b9"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Quốc gia được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("85b73b4f-079c-4e77-b04b-9f4c0c913d79"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Quốc tế được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("9007980a-5ba4-497f-a7f7-ecee5310c146"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("90115bb8-51db-4080-a43e-3383f24a6e8c"), new Guid("ed76e468-43ee-47c3-8148-e6f63406a98d"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("fc948f99-b569-4265-b1c9-ba5aa31d730b"), 23, null, new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("9e99441e-be5b-44dc-b0ca-d75bb9797aa1"), new Guid("8560f2b2-7b9b-4f28-b79a-f5ea21f76e97"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"), 23, null, new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("9eff21e2-bf66-4ff1-a67f-32700c84c412"), new Guid("77daab84-939d-4d0d-957d-27be75bb79b4"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("3da2c117-b32f-4687-89b8-ba9544920f35"), 23, null, new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("a09df10c-a3cd-45d2-ab09-7f7a7a36aeed"), new Guid("3016bf69-e8d1-4852-a717-b5924a7bb7b2"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("fc948f99-b569-4265-b1c9-ba5aa31d730b"), 23, null, new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("aa95d339-ea61-4442-8975-533a7c8432d2"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Trường được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("cd282c91-bc19-4bac-b0e4-6321cf917221"), new Guid("98b05ce5-af6e-4953-be9b-45f97e711c86"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("494e049e-0972-4ff0-a786-6e00880955fc"), 23, null, new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("d203dfe4-b44a-4c37-a17b-726fbacbdd47"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp WoS được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e2ad03e5-11d4-42e4-9626-49db6350bf9a"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Scopus được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e5105b69-738c-493d-8a8d-a2a10f282546"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp WoS được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e78fab66-8637-431d-a2f8-a8d3d81f7124"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn", new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("05a85289-1c41-419f-b8c8-a680833a532d"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("2971a6c0-9bfa-4c62-91db-e290a9008856"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("2c5a2f69-f5c6-493e-968b-3e844154ea0b"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("371136aa-c37c-4f80-88b9-916273b3484d"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("3be58f79-b3c8-4ae2-b761-1d020c064584"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("53b25eec-2419-4cee-a256-c22767dc3a12"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("5eb303f2-4a38-4ecb-a053-6fa2670186fb"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("61502bfe-fba5-47d4-9aa9-c1ec01d7fb0f"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("65a1f78b-0de9-415e-91e9-40c03fb25275"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("83d5e873-4216-4e5e-969d-88cb7aa596b9"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("85b73b4f-079c-4e77-b04b-9f4c0c913d79"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("9007980a-5ba4-497f-a7f7-ecee5310c146"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("90115bb8-51db-4080-a43e-3383f24a6e8c"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("9e99441e-be5b-44dc-b0ca-d75bb9797aa1"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("9eff21e2-bf66-4ff1-a67f-32700c84c412"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("a09df10c-a3cd-45d2-ab09-7f7a7a36aeed"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("aa95d339-ea61-4442-8975-533a7c8432d2"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("cd282c91-bc19-4bac-b0e4-6321cf917221"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("d203dfe4-b44a-4c37-a17b-726fbacbdd47"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("e2ad03e5-11d4-42e4-9626-49db6350bf9a"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("e5105b69-738c-493d-8a8d-a2a10f282546"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("e78fab66-8637-431d-a2f8-a8d3d81f7124"));

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("14b7a7e8-7327-450e-a5ca-f7d836b14499"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b1131264-329f-4908-8e71-8b36088d3dde"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b74daf03-dc04-4738-ae87-97ec0faa07c1"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("d3707663-2b44-4d95-93b7-37756d3e302c"),
                column: "AuthorRoleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("e2f96885-2bb7-4668-9c2f-6d6d313c09f7"),
                column: "AuthorRoleId",
                value: null);
        }
    }
}
