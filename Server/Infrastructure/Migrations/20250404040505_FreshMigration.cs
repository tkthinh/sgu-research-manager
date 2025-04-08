using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FreshMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemConfigs_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcademicTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficerRank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuthorRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMainAuthor = table.Column<bool>(type: "bit", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorRoles_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purposes",
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
                    table.PrimaryKey("PK_Purposes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purposes_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "WorkLevels",
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
                    table.PrimaryKey("PK_WorkLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkLevels_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Assignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurposeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoreLevel = table.Column<int>(type: "int", nullable: true),
                    ConvertHour = table.Column<int>(type: "int", nullable: false),
                    MaxAllowed = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_AuthorRoles_AuthorRoleId",
                        column: x => x.AuthorRoleId,
                        principalTable: "AuthorRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factors_Purposes_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "Purposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_WorkLevels_WorkLevelId",
                        column: x => x.WorkLevelId,
                        principalTable: "WorkLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Factors_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimePublished = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalAuthors = table.Column<int>(type: "int", nullable: true),
                    TotalMainAuthors = table.Column<int>(type: "int", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_WorkLevels_WorkLevelId",
                        column: x => x.WorkLevelId,
                        principalTable: "WorkLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Works_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurposeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SCImagoFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    ScoreLevel = table.Column<int>(type: "int", nullable: true),
                    AuthorHour = table.Column<decimal>(type: "decimal(10,1)", precision: 10, scale: 1, nullable: false),
                    WorkHour = table.Column<int>(type: "int", nullable: false),
                    MarkedForScoring = table.Column<bool>(type: "bit", nullable: false),
                    ProofStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_AuthorRoles_AuthorRoleId",
                        column: x => x.AuthorRoleId,
                        principalTable: "AuthorRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authors_Fields_FieldId",
                        column: x => x.FieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authors_Purposes_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "Purposes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authors_SCImagoFields_SCImagoFieldId",
                        column: x => x.SCImagoFieldId,
                        principalTable: "SCImagoFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authors_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicYearId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorRegistrations_AcademicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorRegistrations_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkAuthors",
                columns: table => new
                {
                    WorkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkAuthors", x => new { x.WorkId, x.UserId });
                    table.ForeignKey(
                        name: "FK_WorkAuthors_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkAuthors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkAuthors_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AcademicYears",
                columns: new[] { "Id", "CreatedDate", "EndDate", "ModifiedDate", "Name", "StartDate" },
                values: new object[,]
                {
                    { new Guid("33fdb5af-0778-4d91-8b68-dce2860e138c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2025-2026", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("dab343ac-b1a8-45b4-a7f8-a4260594d7d8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2024-2025", new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e53bc8e5-a17e-4a9b-a403-0e1b7d3118a2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "2023-2024", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("131d4f64-8e8e-489d-bdd2-36c6920c20bc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA GIÁO DỤC MẦM NON" },
                    { new Guid("2b86577e-5842-4021-bae7-793a1d4d920b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PHÒNG ĐÀO TẠO" },
                    { new Guid("2f86a581-153a-4b49-ae48-997347feb634"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA NGOẠI NGỮ" },
                    { new Guid("334d3f98-43b1-4dbb-9809-eff2c70f0441"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "VĂN PHÒNG" },
                    { new Guid("34d1324e-0f93-4483-83f9-ff0498482555"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA CÔNG NGHỆ THÔNG TIN" },
                    { new Guid("3dec4757-43b9-41e0-92d3-13c2268e5a9f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA GIÁO DỤC QUỐC PHÒNG - AN NINH VÀ GIÁO DỤC THỂ CHẤT" },
                    { new Guid("3e7e47fd-1c04-4641-8beb-10b50b85e209"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA TÀI CHÍNH KẾ TOÁN" },
                    { new Guid("3f39da19-a532-4759-abab-aad4bd56a3f8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "TRẠM Y TẾ" },
                    { new Guid("54ad6a59-caf0-425d-9d0e-24eb62713098"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA LUẬT" },
                    { new Guid("56392386-966f-4366-9769-864a1021b53d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PHÒNG KHẢO THÍ VÀ ĐẢM BẢO CHẤT LƯỢNG GIÁO DỤC" },
                    { new Guid("63391c53-a2cf-4f06-90c6-ead72706aaa9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA GIÁO DỤC TIỂU HỌC" },
                    { new Guid("6fc3ffbd-bc13-4d89-88d9-d0420771461d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PHÒNG GIÁO DỤC THƯỜNG XUYÊN" },
                    { new Guid("7c29b811-4fe7-42d4-a01c-31a48c0c55b8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA MÔI TRƯỜNG" },
                    { new Guid("83751766-bcee-4005-bb14-91767f26fdee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA SƯ PHẠM KHOA HỌC XÃ HỘI" },
                    { new Guid("8cb4057f-7108-44a6-9919-47c2d0669fb7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA GIÁO DỤC CHÍNH TRỊ" },
                    { new Guid("90237856-e82e-48c6-b802-edbe4d467cde"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA VĂN HÓA VÀ DU LỊCH" },
                    { new Guid("94274de8-d2e8-4f3d-9c5c-6941b8c3c604"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA SƯ PHẠM KHOA HỌC TỰ NHIÊN" },
                    { new Guid("b70e6f82-0460-448a-b8a4-7f816db5d0fd"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA TOÁN - ỨNG DỤNG" },
                    { new Guid("bb814b70-df6d-4584-b415-a009230eb3fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA ĐIỆN TỬ VIỄN THÔNG" },
                    { new Guid("cb34108f-043b-4e9f-9568-498f514b3513"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA GIÁO DỤC" },
                    { new Guid("cd88a07d-cb87-4354-8f41-7bdc557b144e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PHÒNG QUẢN LÝ KHOA HỌC" },
                    { new Guid("d32344a0-f267-4ea8-8c24-d3caab71b8aa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PHÒNG ĐÀO TẠO SAU ĐẠI HỌC" },
                    { new Guid("df145d0b-3b4f-4b72-b35b-dbba6e377522"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA QUẢN TRỊ KINH DOANH" },
                    { new Guid("e7ba9cbe-63ed-4efe-a50d-43640b74c92f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "PHÒNG KẾ HOẠCH - TÀI CHÍNH" },
                    { new Guid("ea5be169-45fd-4528-93dc-a53d83f5a1fb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA THƯ VIỆN VĂN PHÒNG" },
                    { new Guid("eb434be4-a7dc-4a13-8eb0-86ab8c01212b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "KHOA NGHỆ THUẬT" }
                });

            migrationBuilder.InsertData(
                table: "Fields",
                columns: new[] { "Id", "CreatedDate", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { new Guid("04bc6c47-e0f4-4176-b047-11a014d20270"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Vật lý" },
                    { new Guid("0ace7c36-6ac9-4d03-8182-132632a7ff4b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sử học - Khảo cổ học - Dân tộc học" },
                    { new Guid("0d79b368-467e-4967-b89f-87e439ba92a6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Xây dựng - Kiến trúc" },
                    { new Guid("222af233-e26e-4e98-a509-4bafa2657512"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Chăn nuôi-Thú y-Thủy sản" },
                    { new Guid("2a2bbf63-f769-4137-8eaf-72a8519dab42"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Ngôn ngữ học" },
                    { new Guid("2b921e8a-8540-4563-946f-de098f1da684"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Văn học" },
                    { new Guid("319bdb13-baa0-41d4-b5a0-77b863f67492"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Công nghệ Thông tin" },
                    { new Guid("32edf4f3-01f0-4531-a51a-4962b11e8f59"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Luật học" },
                    { new Guid("51fe6d6d-f5c5-4992-a7a6-5572dd22562f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa học Trái đất - Mỏ" },
                    { new Guid("549e9619-98e5-4c33-b371-d3eea6866369"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Triết học - Xã hội học - Chính trị học" },
                    { new Guid("59321fed-e04b-45e0-ac81-a8525a01ba04"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Văn hóa - Nghệ thuật - Thể dục thể thao" },
                    { new Guid("5fbfa45d-24e3-40b2-a1e1-12683acb3219"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Toán học" },
                    { new Guid("67d39e1d-7fda-4e2b-8e7a-36b945028cd1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Cơ học" },
                    { new Guid("683650b5-c78c-4dad-adb7-6c49e67340c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giáo dục học" },
                    { new Guid("7121be55-10ff-4976-ae97-ee4cb2e098eb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Điện - Điện tử - Tự động hóa" },
                    { new Guid("727f23c6-3360-4b0c-95b8-67559f95d696"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Cơ khí - Động lực" },
                    { new Guid("7fcac010-dc68-4a9e-9244-4f08af9f5fc2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Thủy lợi" },
                    { new Guid("84670d70-8104-4f36-ab32-cd366dfab481"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa học Quân sự" },
                    { new Guid("86cc7498-2924-436a-9250-0f379de279d7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Giao thông Vận tải" },
                    { new Guid("8d67fae5-ca5f-4630-b581-f93979d7f5ab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Dược học" },
                    { new Guid("8e3a899a-7060-4280-abb7-4fbadc429fd7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Sinh học" },
                    { new Guid("8fcfef89-1f4d-45a9-9062-1f0b2a6dec2c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nông nghiệp - Lâm nghiệp" },
                    { new Guid("baf4bd38-28de-407f-8eb0-44e255eac3b9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Y học" },
                    { new Guid("cefed5af-0f75-4695-8f42-485caa1d9807"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tâm lý học" },
                    { new Guid("db6184ab-8bd3-42e1-a346-a69826e877e2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Luyện kim" },
                    { new Guid("e79de642-e149-4617-8cc0-f6b633b6f5d3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hóa học - Công nghệ thực phẩm" },
                    { new Guid("fcf07bae-9441-44e3-ac81-941eaa8f9762"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khoa học An ninh" },
                    { new Guid("feff8dba-4647-4577-b766-fe5c9f9b68a4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Kinh tế" }
                });

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
                    { new Guid("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Ban Biên tập kỹ yếu", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Trưởng ban", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("5f65dffc-5e3a-46a8-9bc6-1bacce9ef3fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("6304f87f-439a-477d-b989-31df3b6e06b6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "GV hướng dẫn", new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("77daab84-939d-4d0d-957d-27be75bb79b4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("822d8f31-2b1d-4367-8c50-e4535fac5b5f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Phó trưởng ban", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("8560f2b2-7b9b-4f28-b79a-f5ea21f76e97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Chủ biên", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("8bac0cd7-b553-42cc-af1a-5d50d32a6fac"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("98b05ce5-af6e-4953-be9b-45f97e711c86"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("ad3aa473-c140-46cb-b8f4-faecdf2f338e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Ủy viên thường trực", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("b0923868-3ce3-4653-97fa-d6925771ce64"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Tác giả chính", new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("be6b03da-7853-48ab-93b3-81da27c3271e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Đồng chủ biên", new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, "Thành viên", new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, "Ban Chuyên môn", new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
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

            migrationBuilder.InsertData(
                table: "Factors",
                columns: new[] { "Id", "AuthorRoleId", "ConvertHour", "CreatedDate", "MaxAllowed", "ModifiedDate", "Name", "PurposeId", "ScoreLevel", "WorkLevelId", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("0286a29f-f60d-4ded-ad7e-0f145290f36a"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("05644135-f75c-4d65-8c0f-2382c0533880"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("0570292e-3849-4a0e-b66f-32fa34a97f48"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Trường được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("07153ca2-a9cd-44c6-9efa-fba5e65a046f"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("0a45fdcf-34d4-4709-9d55-53e6380c8ccc"), new Guid("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("0d943cc8-f735-4b81-9038-7dfdac675d88"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc tế được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("0f517009-94d4-488a-94d3-1d57bfd21964"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("10b587a5-dff5-4d64-8238-18b45348557d"), new Guid("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("10bc8772-db03-494a-8564-3ddb8c80af4e"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("12fcd9aa-301b-42f0-9195-d1ca60011613"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Giải pháp hữu ích", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 21, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("13610180-0d84-47c8-a280-1d342d52001b"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("14b7a7e8-7327-450e-a5ca-f7d836b14499"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("3da2c117-b32f-4687-89b8-ba9544920f35"), 23, null, new Guid("628a119e-324f-42b8-8ff4-e29ee5c643a9") },
                    { new Guid("166958e9-3e92-40d2-b7e6-f6cf019cbec0"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("166988df-84b4-4b0f-a1e0-8d356a1f4346"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Khuyến khích", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 8, new Guid("08becbaf-2a92-4de1-8908-454c4659ad94"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("173671ca-9de0-4c91-84bb-ffe7dec887e5"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("18f442a7-0df2-4579-ad53-afe90aedf9b3"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("1a015081-cc66-48c4-b6eb-13ffe4aeb756"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("1d749956-707f-4e55-ae63-7e8ad787d716"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("1ebfedcf-12c6-408a-82fd-170f9211d0d3"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Ba", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 9, new Guid("08becbaf-2a92-4de1-8908-454c4659ad94"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("1fabd534-9220-4fed-91bd-5559b286a20c"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 13, new Guid("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("216ac8a5-228f-47c3-a2c7-451fbba219b7"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kết quả nghiên cứu, ứng dụng khoa học", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 22, new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("258a7107-baf3-4632-96be-ada15af33184"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 800, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("2971a16e-2ccc-4f36-a6f6-bf28269c2702"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Thành tích huấn luyện, thi đấu thể dục thể thao", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 17, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("2bd93368-b474-44fc-8146-0bf872f6bb80"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Scopus được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("2c6c95c1-cda0-442b-8257-f2b5b94611a8"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Thành tích huấn luyện, thi đấu thể dục thể thao", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 17, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("2d69276a-cac9-49eb-8623-f5b55c5691d1"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("2f7c8dd0-91e5-4b5d-b1d0-fcd1be18bc54"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("34769b69-b8b3-4bbd-a0a8-c789c333c134"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("34cb99bf-5011-401f-b53c-13448c4ab1bf"), new Guid("ad3aa473-c140-46cb-b8f4-faecdf2f338e"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("35e25540-6ee6-49a1-8188-39b5d8beaa13"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("3a624e20-75c6-47f5-96e5-281ce6c63eaa"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("3f661984-45ac-45af-b665-d7c8f609d172"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 800, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("3f695362-7c44-4f17-a57e-614e68739b94"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 15, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("417cb0bf-ef69-423f-8fa4-3fd4bf3109ad"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("41d45d0a-39ea-417f-ba73-888b495525de"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Nhất", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 11, new Guid("08becbaf-2a92-4de1-8908-454c4659ad94"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("455360e6-693a-47e9-8671-8a83393149ad"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp trường", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("b386e9ba-8844-42eb-b910-6cb360c5485b"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("457f5822-625c-4ce2-81db-c5c5cc99d0ca"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp WoS được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("45ad2da3-47b8-4e71-9248-458192dd52c8"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Thành tích huấn luyện, thi đấu thể dục thể thao", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 18, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("47876171-baff-47c7-ba03-ab35ed7502b0"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("47ac6cea-5dfd-4f40-bb94-7be183fa9421"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("4b04f15e-297a-47a8-a573-28615f19f042"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 640, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("4eb5955a-db99-4a23-99a7-50a94d05ce3c"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("4f68bc47-df55-4fa5-80e7-457e984f4850"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("a210b965-4e0d-41be-a84d-4480bea000f1"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("50d33869-97bc-45a7-a36d-1b031a3c83b5"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kết quả nghiên cứu, ứng dụng khoa học", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 22, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("510e035d-f66a-4489-accd-4c259520c507"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("b2581ebc-a310-460b-9721-f88c92ed2c81"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("54064ab2-4172-4331-9c9f-68a7a384300a"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5590c2f4-733d-4482-85a0-12ed2b76a560"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("56119049-182c-4e4e-8fe2-f0ee3eade9b7"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("568d791f-0a4f-4097-b7e2-9ed9836c3131"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Giải pháp hữu ích", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 20, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("56f1596d-64c5-437d-a22b-853266bbeb93"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("57b568a1-5447-4bb8-bc39-7bc870f86560"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Scopus được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("5a49b577-eb55-4729-a479-0d855d4ce2bd"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 4, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5a94c31b-93bb-43c3-ad13-f7cfc9c8d702"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5aac86cc-aa1e-4a24-895e-f5fe61b76f18"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("b2581ebc-a310-460b-9721-f88c92ed2c81"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("5aeed66d-b2ae-448f-8e30-f7c005c54ff2"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Nhì", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 10, new Guid("6bbf7e31-bcca-4078-b894-7c8d3afba607"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("5e2924fb-9268-4ab0-8527-60096dd3d063"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 640, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5f1f8da3-5209-485d-82fa-9c09db509e74"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 14, new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("5f4545c3-9f56-4f17-8b74-3ea21825fd50"), new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("483f26c2-8218-4d4b-a374-1fbd3a4fc250"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("60772df7-9150-4219-9ee8-ce5439144b0c"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Ba", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 9, new Guid("6bbf7e31-bcca-4078-b894-7c8d3afba607"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("60c6a668-ef9f-4e1f-ad70-737f5f23756c"), new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("483f26c2-8218-4d4b-a374-1fbd3a4fc250"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("61ea9a32-4b56-452d-91ec-cf54ba2ee568"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("62155dde-e5d3-4497-898d-b9765212fade"), new Guid("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("621a47fe-6337-4cf1-a79e-d997627dc1ee"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("62c97efa-acb4-4e13-9887-881d79a57892"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("64780798-ffaf-48eb-be29-8a61fc4854a2"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 20, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH trường hợp còn lại", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 12, new Guid("69cc26ee-f6b8-46a6-9229-e42219775d78"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("6482dbfa-e158-470b-ba51-d6322e7b9684"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("65309514-86ac-4ca2-a957-67616a478f0d"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("68ecc77b-b90c-49cc-adc7-09e5f1257523"), new Guid("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("695a0787-7f2f-48b9-916e-3d1a5f0698c5"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("698099c7-4e76-41e4-a65c-98d07bd7da17"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp WoS được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("6b0be41d-b561-40f8-92cc-0c4957f3fc7c"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("6dd9f77e-a652-4096-8523-71b8f6a4a389"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("6ede2fa2-0b00-4917-91a7-66bea34b8a9a"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("7059bcf8-0d4e-4e9b-840f-33ea4350972c"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc gia được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("722f0278-63c0-4488-973f-dc8baa2729e8"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("73e5510c-66fa-45db-8c3c-245f3bc2b860"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("7493c3a8-923e-45c5-83e3-45baf67107e2"), new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("0e011f57-5ff7-476f-b2bc-46243468fdcb"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("750d273d-b462-4e5c-87cb-266e7a8d201d"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("767bbcc5-cd12-4789-8677-428169b20d48"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Giải pháp hữu ích", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 19, new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("7a633c7f-d43f-4cca-926a-1fa35c2d2e17"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 15, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("7b1f83c1-2d6f-460f-8816-3510673d762a"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 13, new Guid("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("7bebd5b0-0101-420a-b6ac-5b1cdbce04ff"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp WoS được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("7c3cd464-8742-4f93-abff-57073711d1c4"), new Guid("ad3aa473-c140-46cb-b8f4-faecdf2f338e"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("7cc503bc-ad8c-4f97-abe4-7987a115d5d8"), new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("c81240d2-dd87-4949-8252-0116cb5a0cc8"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("7d26c014-1711-459e-8e18-0da00972dd40"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("98c20000-d8e8-4325-93d4-c2d238ac2151"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("7fa77c29-26fb-4b31-b518-b0cb0f37f331"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Thành tích huấn luyện, thi đấu thể dục thể thao", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 18, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("80957f93-d4b1-431b-8287-4c3d55e9d26c"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("81ecbcbc-6b7b-470c-9ae8-ab745c7106fe"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("83cadff5-b7df-4745-81f8-2ee792b3e9b0"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("877b45f8-db8c-4148-84f1-565319312ca2"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("a210b965-4e0d-41be-a84d-4480bea000f1"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("8a94c350-df68-4f57-b57f-ae6dd7a0a3dc"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học cấp Trường được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("8b8fe062-6735-4c5f-a0e6-d0d061737a89"), new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("057a8b2a-7283-43f9-926d-838c7be46987"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("8b9c89a7-d95e-45c9-ae7e-9779eed7a225"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kết quả nghiên cứu, ứng dụng khoa học", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 22, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("8f8a1b3d-3f7e-4fc8-9e8e-9f3f8ead1eaf"), new Guid("822d8f31-2b1d-4367-8c50-e4535fac5b5f"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("902745db-0845-44e7-9f41-f7b44faddb34"), new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("057a8b2a-7283-43f9-926d-838c7be46987"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("90b005be-d7fe-4e6a-acbc-157910884c08"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("918bb01d-f02b-4c07-afda-fbb115b2533d"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("9333b444-ec14-423b-9cab-4a8facf075f5"), new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("0e011f57-5ff7-476f-b2bc-46243468fdcb"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("957e6f12-5644-4b54-ac9f-b582f7b11d7b"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Scopus được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("9632d54d-3a34-4a39-a388-0550a9ca4733"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 640, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("96c19b54-7000-40c4-a6de-e8ecff319ae2"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 16, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("99f88266-0281-49b1-bf44-397cf86816d5"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Giải pháp hữu ích", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 21, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("9ae95c37-54eb-415d-939a-d70008a33b28"), new Guid("ee9e27af-859f-4de6-8678-6ae758654931"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Giải pháp hữu ích", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 20, new Guid("b2302b5e-1614-484d-88ad-003c411ad248"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("9af34412-349c-4cad-a50f-0bf64bdf9eca"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc tế được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("9cde2653-dcc6-4297-91d4-218f0829ae35"), new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("e0264c17-7865-4e6d-b707-6e5227bc63d1"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("9d0bcefd-06c5-427c-a967-64c0a11e2326"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp Cơ sở", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("98c20000-d8e8-4325-93d4-c2d238ac2151"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("9e6cf226-ce18-45f4-86e7-04cded49b021"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("a47f4c75-e5b0-4111-b9d2-beb250aa36a9"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("a4d78d2d-7561-4cfd-b042-c7d37f26df39"), new Guid("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("a523fa26-ee55-4576-92a8-35a7ed52f70e"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc tế được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("a593c6f6-b82a-4415-bae9-309524e6f01e"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 640, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("a80a0cd2-2bab-4e32-8cf3-a56d5a33cacc"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp trường", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("b386e9ba-8844-42eb-b910-6cb360c5485b"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("ad3bdf4e-4697-43bd-a20c-d7ab63e6e59e"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 800, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("aee0c04a-26bc-4ef6-92b7-3d78f6ccaa61"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Nhất", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 11, new Guid("6bbf7e31-bcca-4078-b894-7c8d3afba607"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("b1131264-329f-4908-8e71-8b36088d3dde"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"), 23, null, new Guid("61bbbecc-038a-43b7-aafa-a95e25a93f38") },
                    { new Guid("b1139db5-197e-44a9-a5a1-418a01d36e53"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 600, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b15d18ef-55c6-42ba-815b-9d8855a20563"), new Guid("822d8f31-2b1d-4367-8c50-e4535fac5b5f"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("b2f41bf5-c1f0-488c-872a-82d3f0e06fae"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b51b05e2-774d-476b-b26f-ebf8fc3b90a1"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b52a5fbe-bb11-4026-9d2d-70654e4fefb8"), new Guid("822d8f31-2b1d-4367-8c50-e4535fac5b5f"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("b611282d-b594-4e52-b2c5-058911f8a0fb"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 800, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b7464c59-7774-4f77-9bdb-c3ad962d2067"), new Guid("1c563e5d-0bc0-4861-8ae0-62835d64daa9"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("d588e361-97a2-44cf-a507-24255430dbe7"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("b74daf03-dc04-4738-ae87-97ec0faa07c1"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("fc948f99-b569-4265-b1c9-ba5aa31d730b"), 23, null, new Guid("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") },
                    { new Guid("b986e0d6-36c8-4b73-ab80-566d519bff16"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Khuyến khích", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 8, new Guid("6bbf7e31-bcca-4078-b894-7c8d3afba607"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("b9a61a17-0e15-44a6-b77c-e8804f31bf4b"), new Guid("ad3aa473-c140-46cb-b8f4-faecdf2f338e"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("ba83391f-9d8f-48a9-87d9-b67ebe5be696"), new Guid("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp khoa", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("bae86fa5-c497-4074-8439-db7b54ae6455"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc gia được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("bbd4e0d2-e4ab-4cf8-b4a3-953142b5efc5"), new Guid("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("bc3ae1fa-70eb-4385-a14f-7a820de27846"), new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("e0264c17-7865-4e6d-b707-6e5227bc63d1"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("bec1ee6d-0f3d-4d7c-b868-66b130d1c60b"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c0f3763d-38c5-4016-aef7-9904d77841f0"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 4, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c1233f27-8e66-4c73-9efc-121eb07979f9"), new Guid("e51ba448-a481-4d5e-a560-4b81c45a0530"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Đề tài nghiên cứu cấp khoa", new Guid("b622853d-f917-4871-a3b9-9a1d29ce9506"), null, new Guid("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"), new Guid("49cf7589-fb84-4934-be8e-991c6319a348") },
                    { new Guid("c17fe0b3-0eb8-456c-9e75-ef441bd458c6"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c72c498b-37c6-46cd-b3e1-e82179fd8889"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Giải pháp hữu ích", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 19, new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("c991b3fb-3d52-4896-8ea7-429b21b5dbe9"), new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("c81240d2-dd87-4949-8252-0116cb5a0cc8"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("c99d0edf-12db-45f4-bf9f-722074384101"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 600, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("ca590e0d-797b-46d4-87b7-a223e390b02c"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("cb5db759-7bf6-42d7-affb-9d8a89382cbc"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 16, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("cc99e5e9-e74d-4b21-8603-6e93ddc0c56c"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d1028c26-d4c5-4643-91d1-50824a7ba47e"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d1ddaeeb-532a-4037-9586-05af4446a390"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 600, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d20c4ce0-a360-4f96-889a-49ed20ce0156"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc tế được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("d30ad887-019a-4023-955c-bba587b424d7"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d3707663-2b44-4d95-93b7-37756d3e302c"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("494e049e-0972-4ff0-a786-6e00880955fc"), 23, null, new Guid("3bbfc66a-3144-4edf-959b-e049d7e33d97") },
                    { new Guid("d3c389cc-db9d-4112-878e-48408364b467"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Scopus được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("d4655ab5-d317-4056-a71c-d2d72b2580db"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp WoS được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("d70faba4-e61e-4dae-b683-2e46e73d4578"), new Guid("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"), 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc gia được đăng toàn văn", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("d8f80823-6534-47c1-abaf-2ddeb4c4590b"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 600, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 10% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d9d00031-6a8d-4939-bc95-25a747ceeaec"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d9fc76b8-c3bf-4944-b625-971762b1dff6"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("daff3e2a-5f02-435c-81a7-f4794ce32259"), new Guid("c20d5d29-cf3e-40c5-be56-a2798511c3bc"), 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("dd1ea2c7-4cc5-442d-b8fa-c6a4f8a663a2"), new Guid("73fa58f9-5877-4c31-92b0-ee5665bc0bee"), 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Hướng dẫn đề tài NCKH đạt giải Nhì", new Guid("bf7e1da9-bb9f-4b64-827c-9b5f114395db"), 10, new Guid("08becbaf-2a92-4de1-8908-454c4659ad94"), new Guid("e2f7974c-47c3-478e-9b53-74093f6c621f") },
                    { new Guid("e19e5af4-d301-4881-949b-fa595d175b97"), new Guid("ee9dc844-73d7-458f-9f6d-ae535824c8ca"), 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học cấp Quốc gia được đăng toàn văn", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("e256917b-cb09-4732-bab1-ad10ac407776"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Kết quả nghiên cứu, ứng dụng khoa học", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 22, new Guid("13e5b0a5-727b-427b-b103-0d58db679dcd"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("e2f96885-2bb7-4668-9c2f-6d6d313c09f7"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Sách", new Guid("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"), 23, null, new Guid("84a14a8b-eae8-4720-bc7c-e1f93b35a256") },
                    { new Guid("e88c0e99-9eb0-4218-8310-7d7a95886634"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("ee76f503-a342-4699-a320-cd53a4767322"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học thuộc top 30% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("f029e7f1-69eb-4043-94f7-c9657b383793"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học thuộc top 50% tạp chí hàng đầu", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("f325d698-f4d1-4ab4-915d-f871b509ec92"), new Guid("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("fcac02d5-8f7b-4e0e-aa97-c1d63d86fcc6"), new Guid("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"), 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("bec79373-6f38-4f53-ba87-e986b83ce3b2"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("fda36ff3-5b58-410a-965c-134ffe46f6ab"), new Guid("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"), 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nhiệm vụ biên soạn, chỉnh lý giáo trình", new Guid("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"), null, new Guid("d588e361-97a2-44cf-a507-24255430dbe7"), new Guid("323371c0-26c7-4549-90f2-11c881be402d") },
                    { new Guid("fe73778c-1757-4ad7-92da-470154b1c01d"), new Guid("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"), 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Tham gia tổ chức Hội thảo khoa học", new Guid("f49c3e00-2819-4c03-90ce-b8705555933c"), null, new Guid("071464ae-332b-4426-9b03-cbdd05c2d5bc"), new Guid("140a3e34-ded1-4bfa-8633-fbea545cbdaa") },
                    { new Guid("ff6bd8d0-4772-49fc-a1d5-c2659b19c90e"), new Guid("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Tác phẩm nghệ thuật", new Guid("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"), 14, new Guid("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"), new Guid("1ff8d087-e0c3-45df-befc-662c0a80c10c") },
                    { new Guid("fff69169-c768-4d01-b7d0-360b98899173"), new Guid("069b5046-0a7e-47d9-a8f0-af09db20a697"), 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_DepartmentId",
                table: "Assignments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_UserId",
                table: "Assignments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRegistrations_AcademicYearId_AuthorId",
                table: "AuthorRegistrations",
                columns: new[] { "AcademicYearId", "AuthorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRegistrations_AuthorId",
                table: "AuthorRegistrations",
                column: "AuthorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorRoles_WorkTypeId",
                table: "AuthorRoles",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorRoleId",
                table: "Authors",
                column: "AuthorRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_FieldId",
                table: "Authors",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_PurposeId",
                table: "Authors",
                column: "PurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_SCImagoFieldId",
                table: "Authors",
                column: "SCImagoFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_UserId",
                table: "Authors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_WorkId_UserId",
                table: "Authors",
                columns: new[] { "WorkId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factors_AuthorRoleId",
                table: "Factors",
                column: "AuthorRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_PurposeId",
                table: "Factors",
                column: "PurposeId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_WorkLevelId",
                table: "Factors",
                column: "WorkLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_WorkTypeId",
                table: "Factors",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Purposes_WorkTypeId",
                table: "Purposes",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SCImagoFields_WorkTypeId",
                table: "SCImagoFields",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemConfigs_AcademicYearId",
                table: "SystemConfigs",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FieldId",
                table: "Users",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAuthors_AuthorId",
                table: "WorkAuthors",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkAuthors_UserId",
                table: "WorkAuthors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLevels_WorkTypeId",
                table: "WorkLevels",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkLevelId",
                table: "Works",
                column: "WorkLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_WorkTypeId",
                table: "Works",
                column: "WorkTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "AuthorRegistrations");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "SystemConfigs");

            migrationBuilder.DropTable(
                name: "WorkAuthors");

            migrationBuilder.DropTable(
                name: "AcademicYears");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "AuthorRoles");

            migrationBuilder.DropTable(
                name: "Purposes");

            migrationBuilder.DropTable(
                name: "SCImagoFields");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "WorkLevels");

            migrationBuilder.DropTable(
                name: "WorkTypes");
        }
    }
}
