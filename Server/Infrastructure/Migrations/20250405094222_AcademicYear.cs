using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AcademicYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenDate",
                table: "SystemConfigs",
                newName: "OpenTime");

            migrationBuilder.RenameColumn(
                name: "CloseDate",
                table: "SystemConfigs",
                newName: "CloseTime");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "AcademicYears",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "AcademicYears",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AcademicYears",
                keyColumn: "Id",
                keyValue: new Guid("33fdb5af-0778-4d91-8b68-dce2860e138c"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2026, 6, 30), new DateOnly(2025, 9, 1) });

            migrationBuilder.UpdateData(
                table: "AcademicYears",
                keyColumn: "Id",
                keyValue: new Guid("dab343ac-b1a8-45b4-a7f8-a4260594d7d8"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2025, 6, 30), new DateOnly(2024, 9, 1) });

            migrationBuilder.UpdateData(
                table: "AcademicYears",
                keyColumn: "Id",
                keyValue: new Guid("e53bc8e5-a17e-4a9b-a403-0e1b7d3118a2"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateOnly(2024, 6, 30), new DateOnly(2023, 9, 1) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenTime",
                table: "SystemConfigs",
                newName: "OpenDate");

            migrationBuilder.RenameColumn(
                name: "CloseTime",
                table: "SystemConfigs",
                newName: "CloseDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "AcademicYears",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "AcademicYears",
                keyColumn: "Id",
                keyValue: new Guid("33fdb5af-0778-4d91-8b68-dce2860e138c"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "AcademicYears",
                keyColumn: "Id",
                keyValue: new Guid("dab343ac-b1a8-45b4-a7f8-a4260594d7d8"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "AcademicYears",
                keyColumn: "Id",
                keyValue: new Guid("e53bc8e5-a17e-4a9b-a403-0e1b7d3118a2"),
                columns: new[] { "EndDate", "StartDate" },
                values: new object[] { new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }
    }
}
