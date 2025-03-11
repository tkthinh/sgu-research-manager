using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class udpate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "WorkLevelId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ScoringFieldId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "SCImagoFieldId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "MaxAllowed",
                table: "Factors",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("0fa0aa31-978d-42be-8adc-a47729ff9b9d"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("10c586ce-79ec-4859-881f-5d38245ce47b"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("146e3c5e-7bee-41a6-9f1b-8f00ee2a4eb7"),
                column: "MaxAllowed",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("2ba0b318-cb6f-49a0-b6a3-9040dcc46a9b"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("2fca54b4-555a-4d67-9e9a-f522e0c802cb"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("30775a5a-9282-4801-80f2-acaea95c5f71"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("31673044-6195-4d65-a1f0-453cee604604"),
                column: "MaxAllowed",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("3236b11f-a2ac-4e51-85d1-fcc5594455b6"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("35ba841d-42b2-425a-9e27-82b52a81dc73"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("38f2bbe7-8e32-477a-85bf-e2a08fb88c03"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("3c39ab6a-8c61-4321-b498-4a3a469ea1cc"),
                column: "MaxAllowed",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("415d12ed-7551-48fd-8a5c-87e02fee0dd3"),
                column: "MaxAllowed",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("43454113-745b-4eee-b266-ff515ed9027b"),
                column: "MaxAllowed",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("45892e0d-6abb-41ea-8e29-c34c05b58068"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("4f163c6f-ad7b-4f38-8125-0584678164b6"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("4f47632f-9796-45fa-b4a5-e0c39be496e9"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("5386b122-9311-453e-ab70-de37d9673ef5"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("54740838-3d63-43b4-a498-9c5152dd7528"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("5a01f52c-999f-447e-bb2d-2fb4c9161d25"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("5e382f01-49b7-4910-bf99-cdcddd5042e3"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("60f6bc8a-05ff-4322-bccd-5ea9335139c1"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("630d6ebe-34a2-4533-9a06-ec28ad6d1cd4"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("779bdc74-0043-46a0-84ad-940af4f4dc49"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("849b4d7e-3928-45b6-8f4d-17c078bbcc7f"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("85f22829-055a-4314-a8dc-649a14346610"),
                column: "MaxAllowed",
                value: 8);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("8695bc4f-992a-4b44-ad3e-a373f88672f4"),
                column: "MaxAllowed",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("86f1114c-73b7-4c4f-bac7-95b602bcc397"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("92f2fdef-e1cf-4062-aa96-f01c1820bb98"),
                column: "MaxAllowed",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("9c4ed0a6-b71a-46ea-a27f-02b38bd0c544"),
                column: "MaxAllowed",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("ad619718-0937-4693-a40f-04e56241a52c"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("aeb8a311-304e-4c6e-b944-7cdafac6947b"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b0a80a7a-9703-4af2-b154-bb4a5bd9c315"),
                column: "MaxAllowed",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("b7c82548-b2cd-4368-8b6e-919f7f6b1e5f"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("bd63c850-3c87-4836-9a7e-7c402ad436cf"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("c2288e08-eb6b-4cd0-91ce-9adf4ee8e745"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("c5c436fe-fac7-4243-afd7-a07ba9fa6113"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("c7fc7ef4-dc3e-473f-af05-5514f6c223e8"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("c8785d2b-244a-40ac-9430-7c416adefbc9"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("cbd4799e-ec4b-41a7-8eb2-744841178857"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("ce72ef2c-a629-40ea-9bab-d77b87421fdf"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("d473791e-fb18-409a-be03-a0b60c75912c"),
                column: "MaxAllowed",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("decaf6cd-fb66-440c-9cc3-155036dfc775"),
                column: "MaxAllowed",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("e4f74734-e0f5-446d-8221-cc3a519ad461"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("f56495e4-eac2-47f5-b282-d3f39055fecb"),
                column: "MaxAllowed",
                value: null);

            migrationBuilder.UpdateData(
                table: "Factors",
                keyColumn: "Id",
                keyValue: new Guid("fcd3b42b-7151-4100-8f08-0019c14a764c"),
                column: "MaxAllowed",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxAllowed",
                table: "Factors");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkLevelId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ScoringFieldId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SCImagoFieldId",
                table: "Works",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
