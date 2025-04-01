using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("ba83391f-9d8f-48a9-87d9-b67ebe5be696"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("c1233f27-8e66-4c73-9efc-121eb07979f9"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"));

            migrationBuilder.InsertData(
                table: "Factors",
                columns: new[] { "Id", "AuthorRoleId", "ConvertHour", "CreatedDate", "MaxAllowed", "ModifiedDate", "Name", "PurposeId", "ScoreLevel", "WorkLevelId", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("135f96eb-8ab0-494c-ac9a-c115b470c3d6"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("22505328-67be-49d6-9869-7431bd91f663"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("52e2bd00-fcbb-40a4-b059-4db425c61202"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("89760e99-da91-46ef-bda4-976f3c6572b0"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), 5, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("a575e613-c9ab-44b5-8eb6-33f8914c36d4"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), 5, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("ac12bd71-e645-485d-9f57-6d2efb931657"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b18b9a28-d43d-4258-a3f5-14939ee182ef"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), 5, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d643e931-18d9-4436-a3f6-0e0cb8fc5d38"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("df7fd664-6298-4acf-9094-d0212bf923b6"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("efce39b7-5723-41e7-acd2-0e7bd5b66d1d"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", new Guid("5cf30509-8632-4d62-ad14-55949b9b9336"), null, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") }
                });

            migrationBuilder.InsertData(
                table: "WorkTypes",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[] { new Guid("9787de81-78f2-4797-b810-0f5ec411125b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tài liệu giảng dạy" });

            migrationBuilder.InsertData(
                table: "AuthorRoles",
                columns: new[] { "Id", "CreatedDate", "IsMainAuthor", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("e79d70b3-ea7c-43d8-b441-5317c975ddb3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("9787de81-78f2-4797-b810-0f5ec411125b") },
                    { new Guid("efb42251-5b12-4ac2-b291-e6f8ebd716d4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ nhiệm", new Guid("9787de81-78f2-4797-b810-0f5ec411125b") }
                });

            migrationBuilder.InsertData(
                table: "Purposes",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[] { new Guid("63db31b6-8f8f-4a23-8069-331708fe8ebe"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Quy đổi giờ nghĩa vụ", new Guid("9787de81-78f2-4797-b810-0f5ec411125b") });

            migrationBuilder.InsertData(
                table: "WorkLevels",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[] { new Guid("c6846c10-1297-40c7-820a-76cc286fce31"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa", new Guid("9787de81-78f2-4797-b810-0f5ec411125b") });

            migrationBuilder.InsertData(
                table: "Factors",
                columns: new[] { "Id", "AuthorRoleId", "ConvertHour", "CreatedDate", "MaxAllowed", "ModifiedDate", "Name", "PurposeId", "ScoreLevel", "WorkLevelId", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("a1e70b5f-1670-4254-b0a9-6b3c3936356e"), new Guid("e79d70b3-ea7c-43d8-b441-5317c975ddb3"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tài liệu giảng dạy cấp khoa", new Guid("63db31b6-8f8f-4a23-8069-331708fe8ebe"), null, new Guid("c6846c10-1297-40c7-820a-76cc286fce31"), new Guid("9787de81-78f2-4797-b810-0f5ec411125b") },
                    { new Guid("ec1679cd-c303-4545-a774-82bbdbff81c0"), new Guid("efb42251-5b12-4ac2-b291-e6f8ebd716d4"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tài liệu giảng dạy cấp khoa", new Guid("63db31b6-8f8f-4a23-8069-331708fe8ebe"), null, new Guid("c6846c10-1297-40c7-820a-76cc286fce31"), new Guid("9787de81-78f2-4797-b810-0f5ec411125b") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("135f96eb-8ab0-494c-ac9a-c115b470c3d6"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("22505328-67be-49d6-9869-7431bd91f663"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("52e2bd00-fcbb-40a4-b059-4db425c61202"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("89760e99-da91-46ef-bda4-976f3c6572b0"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("a1e70b5f-1670-4254-b0a9-6b3c3936356e"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("a575e613-c9ab-44b5-8eb6-33f8914c36d4"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("ac12bd71-e645-485d-9f57-6d2efb931657"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b18b9a28-d43d-4258-a3f5-14939ee182ef"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("d643e931-18d9-4436-a3f6-0e0cb8fc5d38"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("df7fd664-6298-4acf-9094-d0212bf923b6"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("ec1679cd-c303-4545-a774-82bbdbff81c0"));

            migrationBuilder.DeleteData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("efce39b7-5723-41e7-acd2-0e7bd5b66d1d"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("e79d70b3-ea7c-43d8-b441-5317c975ddb3"));

            migrationBuilder.DeleteData(
                table: "AuthorRoles",
                keyColumn: "Id",
                keyValue: new Guid("efb42251-5b12-4ac2-b291-e6f8ebd716d4"));

            migrationBuilder.DeleteData(
                table: "Purposes",
                keyColumn: "Id",
                keyValue: new Guid("63db31b6-8f8f-4a23-8069-331708fe8ebe"));

            migrationBuilder.DeleteData(
                table: "WorkLevels",
                keyColumn: "Id",
                keyValue: new Guid("c6846c10-1297-40c7-820a-76cc286fce31"));

            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValue: new Guid("9787de81-78f2-4797-b810-0f5ec411125b"));

            migrationBuilder.InsertData(
                table: "WorkLevels",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name", "WorkTypeId" },
                values: new object[] { new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") });

            migrationBuilder.InsertData(
                table: "Factors",
                columns: new[] { "Id", "AuthorRoleId", "ConvertHour", "CreatedDate", "MaxAllowed", "ModifiedDate", "Name", "PurposeId", "ScoreLevel", "WorkLevelId", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("ba83391f-9d8f-48a9-87d9-b67ebe5be696"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp khoa", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("c1233f27-8e66-4c73-9efc-121eb07979f9"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp khoa", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") }
                });
        }
    }
}
