using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removerelationshipscimagofildwithworktype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SCImagoFields_WorkTypes_WorkTypeId",
                table: "SCImagoFields");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkTypeId",
                table: "SCImagoFields",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("401fcfa1-b021-47a9-876e-4c2af8ebb470"),
                columns: new[] { "Name", "WorkTypeId" },
                values: new object[] { "Analysis", null });

            migrationBuilder.UpdateData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("5d650d2a-dcbf-4efc-bb3d-9cdbce0c207e"),
                columns: new[] { "Name", "WorkTypeId" },
                values: new object[] { "Anatomy", null });

            migrationBuilder.UpdateData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("71d16d65-5823-41b6-975d-b8189c41481b"),
                column: "WorkTypeId",
                value: null);

            migrationBuilder.AddForeignKey(
                name: "FK_SCImagoFields_WorkTypes_WorkTypeId",
                table: "SCImagoFields",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SCImagoFields_WorkTypes_WorkTypeId",
                table: "SCImagoFields");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkTypeId",
                table: "SCImagoFields",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("401fcfa1-b021-47a9-876e-4c2af8ebb470"),
                columns: new[] { "Name", "WorkTypeId" },
                values: new object[] { "Accounting", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") });

            migrationBuilder.UpdateData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("5d650d2a-dcbf-4efc-bb3d-9cdbce0c207e"),
                columns: new[] { "Name", "WorkTypeId" },
                values: new object[] { "Accounting", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") });

            migrationBuilder.UpdateData(
                table: "SCImagoFields",
                keyColumn: "Id",
                keyValue: new Guid("71d16d65-5823-41b6-975d-b8189c41481b"),
                column: "WorkTypeId",
                value: new Guid("2732c858-77dc-471d-bd9a-464a3142530a"));

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
                    { new Guid("5e4f7453-8d53-4af7-a14c-d4539abbc2b4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Advanced and Specialized Nursing", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("69b6c01d-af65-4bc7-afc3-a0a917fc0e4c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Analysis", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("6b652d84-6e79-4a60-b8ee-8b07f1da0fae"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Aerospace Engineering", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("6d266310-a83e-4367-b1ef-a331e475db7e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Algebra and Number Theory", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("6dbe6812-1775-429e-97f4-e39526e8d95d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("70396506-ba86-410f-b09e-7db24e1f7b19"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Acoustics and Ultrasonics", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("71b2b969-44a1-4953-994f-693c851e0bf6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Agronomy and Crop Science", new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
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

            migrationBuilder.AddForeignKey(
                name: "FK_SCImagoFields_WorkTypes_WorkTypeId",
                table: "SCImagoFields",
                column: "WorkTypeId",
                principalTable: "WorkTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
