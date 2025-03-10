using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkTypes",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("03412ca7-8ccf-4903-9018-457768060ab4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Báo cáo khoa học" },
                    { new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hội thảo, hội nghị" },
                    { new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khác" },
                    { new Guid("2732c858-77dc-471d-bd9a-464a3142530a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bài báo khoa học" },
                    { new Guid("323371c0-26c7-4549-90f2-11c881be402d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giáo trình" },
                    { new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Chương sách" },
                    { new Guid("49cf7589-fb84-4934-be8e-991c6319a348"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Đề tài" },
                    { new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Chuyên khảo" },
                    { new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tham khảo" },
                    { new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giáo trình - Sách" },
                    { new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tài liệu hướng dẫn" },
                    { new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hướng dẫn SV NCKH" }
                });

            migrationBuilder.InsertData(
                table: "AuthorRoles",
                columns: new[] { "Id", "CreatedDate", "IsMainAuthor", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Tác giả chính", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("11eea600-4495-486c-985d-57de08b8b5da"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("3016bf69-e8d1-4852-a717-b5924a7bb7b2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("3dfd761c-256e-442f-99fb-136d27b4cea5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("3f0d8b5e-99da-4702-bc34-1b36c99cbdaa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("4a85d698-7809-4912-923f-18c3f0a2e676"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Ban Biên tập kỹ yếu", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Trưởng ban", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("5f65dffc-5e3a-46a8-9bc6-1bacce9ef3fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("6304f87f-439a-477d-b989-31df3b6e06b6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("77daab84-939d-4d0d-957d-27be75bb79b4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("822d8f31-2b1d-4367-8c50-e4535fac5b5f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Phó trưởng ban", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("8560f2b2-7b9b-4f28-b79a-f5ea21f76e97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("8bac0cd7-b553-42cc-af1a-5d50d32a6fac"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("98b05ce5-af6e-4953-be9b-45f97e711c86"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("ad3aa473-c140-46cb-b8f4-faecdf2f338e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Ủy viên thường trực", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("b0923868-3ce3-4653-97fa-d6925771ce64"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Tác giả chính", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("be6b03da-7853-48ab-93b3-81da27c3271e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Ban Chuyên môn", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("d8d1af53-3354-4af3-a18f-85c6ee46e750"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ nhiệm", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("ed76e468-43ee-47c3-8148-e6f63406a98d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Tác giả chính", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") }
                });

            migrationBuilder.InsertData(
                table: "Purposes",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("3da2c117-b32f-4687-89b8-ba9544920f35"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("494e049e-0972-4ff0-a786-6e00880955fc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sản phẩm của đề tài NCKH", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sản phẩm của đề tài NCKH", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi vượt định mức", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi vượt định mức", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("fc948f99-b569-4265-b1c9-ba5aa31d730b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }
                });

            migrationBuilder.InsertData(
                table: "SCImagoFields",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("0546a881-8ac0-4ff3-9145-672ad8ee1384"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("05552af0-6e6d-40cd-8dcd-90204f20bfca"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("05ccc5da-0496-46c8-ada2-5d6a0e466536"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("08852f72-cfc1-4e62-ba27-5d33dc5b894f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("0bfe40c5-db06-41a9-8a20-8fefc7b8bc56"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("173a0f0e-9516-488c-a5dd-531478e7842f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("17fa40fc-295f-47b8-a573-090b280fb201"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("1aad5fb3-4742-43a6-b004-d38cea7554e5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("2122ea3c-d666-45d9-933a-57e0c853d77a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("21dc2bc6-9a66-4126-8df4-550bf46e834b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("2566aaaf-185b-424a-ac0a-4373f08be1cd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("26ed0b8a-e453-4882-b2c7-9d8b18baca4e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("2d518f0c-4611-426e-883d-4192bda56371"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("2df7bd79-06fe-4c34-89c5-0c8c9aa99300"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("30120b72-4c68-46da-9cfd-c275c87c5b4b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("30ef4e0a-f1c4-4e02-b81f-96debeea8ba7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("31f4e8a8-e50c-46a8-8a69-07d15fea8374"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("3657d8a5-9ca3-4310-af30-8c919f1d0ddc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("37d326bf-2bdf-44b9-9fac-043066058006"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("38cabc00-a4ec-4a5e-abd3-51394cdcdb1d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("397ff495-3f2c-448a-95ca-0ef9eaecd493"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("39f200aa-04e3-4d19-b60d-f0167e5901af"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("3a6733ce-1858-4628-b34d-0b96ebe3a6c1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("3b550a89-4f41-4338-9b59-86e125d799e8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("3cfc940b-753d-4286-b2cc-274108045404"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("401a8622-0680-495d-8da6-47e739effd62"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("401fcfa1-b021-47a9-876e-4c2af8ebb470"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("41e1107a-6f87-493b-ac8e-13479ef48fb9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("429f30db-d831-4909-92d9-8642ac476c5d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("496579af-c43f-4aee-987e-f7bd5ee7fc4e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("4cdc5166-08dc-4eb7-acb0-9e0c2c9547b7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("4e32a1e4-1360-438b-87d8-2f4f273dd01e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("4ea5768f-e908-41d1-875a-fafe00d072d6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("514a1695-a534-4c68-85f3-b7d7f3c2cf6a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("53c9682c-54c6-4515-9414-7ed2a5ab9dbd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("566ebe27-f2e3-431c-91b3-36864f9531cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("59344f92-394a-49a7-afbc-673731e2beed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("59361118-b41c-4718-88c1-16aac146337a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("5d650d2a-dcbf-4efc-bb3d-9cdbce0c207e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("5e4f7453-8d53-4af7-a14c-d4539abbc2b4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("69b6c01d-af65-4bc7-afc3-a0a917fc0e4c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("6b652d84-6e79-4a60-b8ee-8b07f1da0fae"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("6d266310-a83e-4367-b1ef-a331e475db7e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("6dbe6812-1775-429e-97f4-e39526e8d95d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("70396506-ba86-410f-b09e-7db24e1f7b19"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("71b2b969-44a1-4953-994f-693c851e0bf6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("71d16d65-5823-41b6-975d-b8189c41481b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("72b465d7-e634-4878-a3dc-d42165da4f20"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("736edc0a-1918-486a-8508-be2a3729bbe6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("73bd013c-6d0a-4b32-aeac-b1df414ad8be"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("73c183be-4203-4c11-be2b-1eb327f61a4b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("76842dc6-38a9-4ec6-9ac8-20f298eb09a1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("7aa65d95-c4e7-457d-aa3b-e9df7684fe4c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("7beec1fe-625c-4461-804b-7dc40e6e34dc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("7edc3d54-6f58-4b39-8252-1d02fe836d18"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("80435537-dbfd-49b0-8952-d9e6c67289b4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("81fbf356-d99b-4911-b596-3b723910c5de"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("83409cd3-b669-413c-a71c-7a1a0ff761c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("86237475-2dd4-4c6a-b37a-f5d9dcd235a4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("8b1416d0-6265-4284-b6ec-db01db76e59e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("8c1d9602-dff1-4592-8ffc-3abf18f83707"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("92433e34-b419-48e0-ac63-45bc4303c5b3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("9520ec93-3438-4576-804e-0a3b0dd5d9ab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("968bfb7c-3371-4a66-9fa0-7e6bba2e6bc7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("96c82acf-938d-41af-bb1b-716e8136a925"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("9ca69376-adbd-4e06-959e-364e739d5e1d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("9d81951c-19b7-425d-9e35-c39a9251b1c1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("9e2e9363-7202-4fec-afe7-ebef07a882e6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("a25484e5-bcb4-47f7-8a86-a4f8cd488b3a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("a4c01d6d-bc55-4f8a-88f3-f0caa52019e1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("a803230c-efa0-49d9-99d6-b8d3c7c9bc48"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("a94f435c-ba53-4350-890d-77d7a38ab197"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("acc93d1f-a550-4f69-9cf4-eb277421e0c3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("ad36a77b-bbbd-497e-8929-8a6703a3e397"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aging", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("ad5f1ec7-f451-4122-918f-0d389e4293a3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("b0de64d1-ce89-4800-b24a-c2d2ef327ef5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b1340f55-49a9-45d7-bd81-7c6e6c156dbc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("b4aa636d-c892-446a-a2d0-bace85fa681c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("b683926d-a267-4ad3-b387-ce17eab9acb9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("b6bf371a-d91f-43d1-8920-12db377ff70f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("b81a7ded-6768-4d94-aad1-e9f60e8d9d60"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("b91e4e0b-a3e1-4694-ac6e-041a259f98e9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("b9fd2cc9-cdd8-405c-b616-20e16dcd8fc0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Accounting", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("bd40abcc-83cb-4b04-b907-9f5d14aaa736"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("cc970669-ecbb-43e9-829c-6ef73382b868"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("cca5e727-1820-4f59-a929-ab7932e97830"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("cf9737f8-e9af-42ff-9a6a-9791602676ad"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("d1aa9f38-b6a0-46f4-a099-a1f5a2c3612a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("d825ee2a-83e0-4692-a7ca-8987be1926a2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("d828e4c5-6fec-43fd-914a-d7860f349874"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d8966118-e417-485d-870a-ba0c35581413"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e0da499d-f636-463f-b407-c503754687d9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("e249a273-360b-40bb-bb52-3b2e066bf648"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e607986e-e8b7-44d6-8ef9-ed7bf3796e97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("e73c1b54-61b8-4ecf-a831-523b12926c17"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("ed1fae08-4468-49c2-85f0-ce964ab80d2b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("f2005a34-e48e-47b1-a4af-53396d4bc96c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analytical Chemistry", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("f54ed2ce-4385-486f-a96f-203aff849298"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("f7d2ace6-bad7-49d5-af0b-73b49678483e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("f97d98e9-5eb6-4b82-9613-ad2e88988a3a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agricultural and Biological Sciences (miscellaneous)", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") }
                });

            migrationBuilder.InsertData(
                table: "WorkLevels",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bộ/Ngành", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("057a8b2a-7283-43f9-926d-838c7be46987"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc tế", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("08becbaf-2a92-4de1-8908-454c4659ad94"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Eureka", new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "WoS", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("0e011f57-5ff7-476f-b2bc-46243468fdcb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc tế", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bộ/Ngành", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bộ/Ngành", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Scopus", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("483f26c2-8218-4d4b-a374-1fbd3a4fc250"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("69cc26ee-f6b8-46a6-9229-e42219775d78"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("6bbf7e31-bcca-4078-b894-7c8d3afba607"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bộ", new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc gia", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("98c20000-d8e8-4325-93d4-c2d238ac2151"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tỉnh/Thành phố", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("a210b965-4e0d-41be-a84d-4480bea000f1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nhà nước", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc tế", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc gia", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("b2581ebc-a310-460b-9721-f88c92ed2c81"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc tế", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("b386e9ba-8844-42eb-b910-6cb360c5485b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc tế", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("c81240d2-dd87-4949-8252-0116cb5a0cc8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tỉnh/Thành phố", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("d588e361-97a2-44cf-a507-24255430dbe7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bộ/Ngành", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tỉnh/Thành phố", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc gia", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quốc tế", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e0264c17-7865-4e6d-b707-6e5227bc63d1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nhà nước", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trường", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Scopus", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "WoS", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("11eea600-4495-486c-985d-57de08b8b5da"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("3016bf69-e8d1-4852-a717-b5924a7bb7b2"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("3dfd761c-256e-442f-99fb-136d27b4cea5"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f0d8b5e-99da-4702-bc34-1b36c99cbdaa"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("4a85d698-7809-4912-923f-18c3f0a2e676"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("5f65dffc-5e3a-46a8-9bc6-1bacce9ef3fa"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("6304f87f-439a-477d-b989-31df3b6e06b6"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("77daab84-939d-4d0d-957d-27be75bb79b4"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("822d8f31-2b1d-4367-8c50-e4535fac5b5f"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("8560f2b2-7b9b-4f28-b79a-f5ea21f76e97"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("8bac0cd7-b553-42cc-af1a-5d50d32a6fac"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("98b05ce5-af6e-4953-be9b-45f97e711c86"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("ad3aa473-c140-46cb-b8f4-faecdf2f338e"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("b0923868-3ce3-4653-97fa-d6925771ce64"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("be6b03da-7853-48ab-93b3-81da27c3271e"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("d8d1af53-3354-4af3-a18f-85c6ee46e750"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("ed76e468-43ee-47c3-8148-e6f63406a98d"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("ee9e27af-859f-4de6-8678-6ae758654931"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("3da2c117-b32f-4687-89b8-ba9544920f35"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("494e049e-0972-4ff0-a786-6e00880955fc"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("be2cb497-02ac-4f5c-ae8a-062876730a2b"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("fc948f99-b569-4265-b1c9-ba5aa31d730b"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("0546a881-8ac0-4ff3-9145-672ad8ee1384"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("05552af0-6e6d-40cd-8dcd-90204f20bfca"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("05ccc5da-0496-46c8-ada2-5d6a0e466536"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("08852f72-cfc1-4e62-ba27-5d33dc5b894f"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("0bfe40c5-db06-41a9-8a20-8fefc7b8bc56"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("173a0f0e-9516-488c-a5dd-531478e7842f"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("17fa40fc-295f-47b8-a573-090b280fb201"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("1aad5fb3-4742-43a6-b004-d38cea7554e5"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("2122ea3c-d666-45d9-933a-57e0c853d77a"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("21dc2bc6-9a66-4126-8df4-550bf46e834b"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("2566aaaf-185b-424a-ac0a-4373f08be1cd"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("26ed0b8a-e453-4882-b2c7-9d8b18baca4e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("2d518f0c-4611-426e-883d-4192bda56371"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("2df7bd79-06fe-4c34-89c5-0c8c9aa99300"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("30120b72-4c68-46da-9cfd-c275c87c5b4b"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("30ef4e0a-f1c4-4e02-b81f-96debeea8ba7"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("31f4e8a8-e50c-46a8-8a69-07d15fea8374"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("3657d8a5-9ca3-4310-af30-8c919f1d0ddc"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("37d326bf-2bdf-44b9-9fac-043066058006"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("38cabc00-a4ec-4a5e-abd3-51394cdcdb1d"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("397ff495-3f2c-448a-95ca-0ef9eaecd493"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("39f200aa-04e3-4d19-b60d-f0167e5901af"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("3a6733ce-1858-4628-b34d-0b96ebe3a6c1"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("3b550a89-4f41-4338-9b59-86e125d799e8"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("3cfc940b-753d-4286-b2cc-274108045404"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("401a8622-0680-495d-8da6-47e739effd62"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("401fcfa1-b021-47a9-876e-4c2af8ebb470"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("41e1107a-6f87-493b-ac8e-13479ef48fb9"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("429f30db-d831-4909-92d9-8642ac476c5d"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("496579af-c43f-4aee-987e-f7bd5ee7fc4e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("4cdc5166-08dc-4eb7-acb0-9e0c2c9547b7"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("4e32a1e4-1360-438b-87d8-2f4f273dd01e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("4ea5768f-e908-41d1-875a-fafe00d072d6"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("514a1695-a534-4c68-85f3-b7d7f3c2cf6a"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("53c9682c-54c6-4515-9414-7ed2a5ab9dbd"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("566ebe27-f2e3-431c-91b3-36864f9531cb"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("59344f92-394a-49a7-afbc-673731e2beed"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("59361118-b41c-4718-88c1-16aac146337a"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("5d650d2a-dcbf-4efc-bb3d-9cdbce0c207e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("5e4f7453-8d53-4af7-a14c-d4539abbc2b4"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("69b6c01d-af65-4bc7-afc3-a0a917fc0e4c"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("6b652d84-6e79-4a60-b8ee-8b07f1da0fae"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("6d266310-a83e-4367-b1ef-a331e475db7e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("6dbe6812-1775-429e-97f4-e39526e8d95d"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("70396506-ba86-410f-b09e-7db24e1f7b19"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("71b2b969-44a1-4953-994f-693c851e0bf6"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("71d16d65-5823-41b6-975d-b8189c41481b"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("72b465d7-e634-4878-a3dc-d42165da4f20"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("736edc0a-1918-486a-8508-be2a3729bbe6"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("73bd013c-6d0a-4b32-aeac-b1df414ad8be"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("73c183be-4203-4c11-be2b-1eb327f61a4b"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("76842dc6-38a9-4ec6-9ac8-20f298eb09a1"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("7aa65d95-c4e7-457d-aa3b-e9df7684fe4c"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("7beec1fe-625c-4461-804b-7dc40e6e34dc"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("7edc3d54-6f58-4b39-8252-1d02fe836d18"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("80435537-dbfd-49b0-8952-d9e6c67289b4"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("81fbf356-d99b-4911-b596-3b723910c5de"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("83409cd3-b669-413c-a71c-7a1a0ff761c5"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("86237475-2dd4-4c6a-b37a-f5d9dcd235a4"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("8b1416d0-6265-4284-b6ec-db01db76e59e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("8c1d9602-dff1-4592-8ffc-3abf18f83707"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("92433e34-b419-48e0-ac63-45bc4303c5b3"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("9520ec93-3438-4576-804e-0a3b0dd5d9ab"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("968bfb7c-3371-4a66-9fa0-7e6bba2e6bc7"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("96c82acf-938d-41af-bb1b-716e8136a925"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("9ca69376-adbd-4e06-959e-364e739d5e1d"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("9d81951c-19b7-425d-9e35-c39a9251b1c1"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("9e2e9363-7202-4fec-afe7-ebef07a882e6"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("a25484e5-bcb4-47f7-8a86-a4f8cd488b3a"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("a4c01d6d-bc55-4f8a-88f3-f0caa52019e1"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("a803230c-efa0-49d9-99d6-b8d3c7c9bc48"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("a94f435c-ba53-4350-890d-77d7a38ab197"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("acc93d1f-a550-4f69-9cf4-eb277421e0c3"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("ad36a77b-bbbd-497e-8929-8a6703a3e397"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("ad5f1ec7-f451-4122-918f-0d389e4293a3"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b0de64d1-ce89-4800-b24a-c2d2ef327ef5"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b1340f55-49a9-45d7-bd81-7c6e6c156dbc"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b4aa636d-c892-446a-a2d0-bace85fa681c"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b683926d-a267-4ad3-b387-ce17eab9acb9"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b6bf371a-d91f-43d1-8920-12db377ff70f"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b81a7ded-6768-4d94-aad1-e9f60e8d9d60"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b91e4e0b-a3e1-4694-ac6e-041a259f98e9"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("b9fd2cc9-cdd8-405c-b616-20e16dcd8fc0"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("bd40abcc-83cb-4b04-b907-9f5d14aaa736"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("cc970669-ecbb-43e9-829c-6ef73382b868"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("cca5e727-1820-4f59-a929-ab7932e97830"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("cf9737f8-e9af-42ff-9a6a-9791602676ad"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("d1aa9f38-b6a0-46f4-a099-a1f5a2c3612a"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("d825ee2a-83e0-4692-a7ca-8987be1926a2"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("d828e4c5-6fec-43fd-914a-d7860f349874"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("d8966118-e417-485d-870a-ba0c35581413"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("e0da499d-f636-463f-b407-c503754687d9"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("e249a273-360b-40bb-bb52-3b2e066bf648"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("e607986e-e8b7-44d6-8ef9-ed7bf3796e97"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("e73c1b54-61b8-4ecf-a831-523b12926c17"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("ed1fae08-4468-49c2-85f0-ce964ab80d2b"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("f2005a34-e48e-47b1-a4af-53396d4bc96c"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("f54ed2ce-4385-486f-a96f-203aff849298"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("f7d2ace6-bad7-49d5-af0b-73b49678483e"));

            migrationBuilder.DeleteData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("f97d98e9-5eb6-4b82-9613-ad2e88988a3a"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("057a8b2a-7283-43f9-926d-838c7be46987"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("08becbaf-2a92-4de1-8908-454c4659ad94"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("0e011f57-5ff7-476f-b2bc-46243468fdcb"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("23dad081-62db-4944-87d2-43b29c31fa29"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("483f26c2-8218-4d4b-a374-1fbd3a4fc250"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("69cc26ee-f6b8-46a6-9229-e42219775d78"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("6bbf7e31-bcca-4078-b894-7c8d3afba607"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("740e8212-f47b-4080-b57a-839b8b90056c"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("98c20000-d8e8-4325-93d4-c2d238ac2151"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("a210b965-4e0d-41be-a84d-4480bea000f1"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("b2302b5e-1614-484d-88ad-003c411ad248"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("b2581ebc-a310-460b-9721-f88c92ed2c81"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("b386e9ba-8844-42eb-b910-6cb360c5485b"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("c81240d2-dd87-4949-8252-0116cb5a0cc8"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("d588e361-97a2-44cf-a507-24255430dbe7"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("e0264c17-7865-4e6d-b707-6e5227bc63d1"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("03412ca7-8ccf-4903-9018-457768060ab4"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("2732c858-77dc-471d-bd9a-464a3142530a"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("323371c0-26c7-4549-90f2-11c881be402d"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("49cf7589-fb84-4934-be8e-991c6319a348"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f"));
        }
    }
}
