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
                name: "SystemConfigs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemConfigs", x => x.Id);
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficerRank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdentityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        onDelete: ReferentialAction.Cascade);
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
                    FinalWorkHour = table.Column<int>(type: "int", nullable: false),
                    ProofStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SCImagoFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScoringFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Works_Fields_ScoringFieldId",
                        column: x => x.ScoringFieldId,
                        principalTable: "Fields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    AuthorRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurposeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: true),
                    ScoreLevel = table.Column<int>(type: "int", nullable: true),
                    FinalAuthorHour = table.Column<int>(type: "int", nullable: false),
                    TempAuthorHour = table.Column<int>(type: "int", nullable: false),
                    TempWorkHour = table.Column<int>(type: "int", nullable: false),
                    MarkedForScoring = table.Column<bool>(type: "bit", nullable: false),
                    CoAuthors = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                        name: "FK_Authors_Purposes_PurposeId",
                        column: x => x.PurposeId,
                        principalTable: "Purposes",
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

            migrationBuilder.InsertData(
                table: "Factors",
                columns: new[] { "Id", "AuthorRoleId", "ConvertHour", "CreatedDate", "MaxAllowed", "ModifiedDate", "Name", "PurposeId", "ScoreLevel", "WorkLevelId", "WorkTypeId" },
                values: new object[,]
                {
                    { new Guid("0fa0aa31-978d-42be-8adc-a47729ff9b9d"), null, 640, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 30", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("10c586ce-79ec-4859-881f-5d38245ce47b"), null, 800, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 10", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 4, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("146e3c5e-7bee-41a6-9f1b-8f00ee2a4eb7"), null, 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Báo cáo khoa học - Bộ/Ngành - Quy đổi vượt định mức", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("2ba0b318-cb6f-49a0-b6a3-9040dcc46a9b"), null, 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học - Quốc gia - Quy đổi giờ nghĩa vụ", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("2fca54b4-555a-4d67-9e9a-f522e0c802cb"), null, 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 30", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("30775a5a-9282-4801-80f2-acaea95c5f71"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("31673044-6195-4d65-a1f0-453cee604604"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Báo cáo khoa học - Scopus - Quy đổi vượt định mức", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("3236b11f-a2ac-4e51-85d1-fcc5594455b6"), null, 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Bộ/Ngành - Quy đổi vượt định mức - Score 0.5", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("35ba841d-42b2-425a-9e27-82b52a81dc73"), null, 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 100", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("38f2bbe7-8e32-477a-85bf-e2a08fb88c03"), null, 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 50", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("3c39ab6a-8c61-4321-b498-4a3a469ea1cc"), null, 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, "Báo cáo khoa học - Quốc gia - Quy đổi vượt định mức", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("740e8212-f47b-4080-b57a-839b8b90056c"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("415d12ed-7551-48fd-8a5c-87e02fee0dd3"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Báo cáo khoa học - WoS - Quy đổi vượt định mức", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("43454113-745b-4eee-b266-ff515ed9027b"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 50", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("45892e0d-6abb-41ea-8e29-c34c05b58068"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ - Score 0.75", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("4f163c6f-ad7b-4f38-8125-0584678164b6"), null, 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Báo cáo khoa học - Quốc tế - Quy đổi vượt định mức", new Guid("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"), null, new Guid("db324190-d1ed-4712-b3db-94a6e043bf1e"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("4f47632f-9796-45fa-b4a5-e0c39be496e9"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Trường - Quy đổi giờ nghĩa vụ - Score 0.75", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5386b122-9311-453e-ab70-de37d9673ef5"), null, 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học - Trường - Quy đổi vượt định mức - Score 0.5", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("54740838-3d63-43b4-a498-9c5152dd7528"), null, 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học - Quốc tế - Quy đổi vượt định mức - Score 0.75", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5a01f52c-999f-447e-bb2d-2fb4c9161d25"), null, 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học - Trường - Quy đổi vượt định mức - Score 0.75", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("5e382f01-49b7-4910-bf99-cdcddd5042e3"), null, 640, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 30", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 5, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("60f6bc8a-05ff-4322-bccd-5ea9335139c1"), null, 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("250662c1-1c69-4ef0-a21d-7077cafd1d06"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("630d6ebe-34a2-4533-9a06-ec28ad6d1cd4"), null, 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ - Score 1", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("779bdc74-0043-46a0-84ad-940af4f4dc49"), null, 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Trường - Quy đổi giờ nghĩa vụ - Score 1", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("849b4d7e-3928-45b6-8f4d-17c078bbcc7f"), null, 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 30", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 5, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("85f22829-055a-4314-a8dc-649a14346610"), null, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 8, null, "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 50", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 6, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("8695bc4f-992a-4b44-ad3e-a373f88672f4"), null, 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học - Quốc tế - Quy đổi vượt định mức - Score 1", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("86f1114c-73b7-4c4f-bac7-95b602bcc397"), null, 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học - Scopus - Quy đổi giờ nghĩa vụ", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("92f2fdef-e1cf-4062-aa96-f01c1820bb98"), null, 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học - Trường - Quy đổi vượt định mức - Score 1", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("9c4ed0a6-b71a-46ea-a27f-02b38bd0c544"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 100", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("ad619718-0937-4693-a40f-04e56241a52c"), null, 40, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học - Trường - Quy đổi giờ nghĩa vụ", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("3c21b247-16ce-40a9-a921-abef0e1bba56"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("aeb8a311-304e-4c6e-b944-7cdafac6947b"), null, 560, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 50", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 6, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b0a80a7a-9703-4af2-b154-bb4a5bd9c315"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, null, "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 100", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 7, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("b7c82548-b2cd-4368-8b6e-919f7f6b1e5f"), null, 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ - Score 0.5", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("bd63c850-3c87-4836-9a7e-7c402ad436cf"), null, 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Trường - Quy đổi giờ nghĩa vụ - Score 0.5", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("23dad081-62db-4944-87d2-43b29c31fa29"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c2288e08-eb6b-4cd0-91ce-9adf4ee8e745"), null, 600, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 10", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c5c436fe-fac7-4243-afd7-a07ba9fa6113"), null, 600, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 10", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 4, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c7fc7ef4-dc3e-473f-af05-5514f6c223e8"), null, 800, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 10", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 4, new Guid("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("c8785d2b-244a-40ac-9430-7c416adefbc9"), null, 120, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ - Score 0.75", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 2, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("cbd4799e-ec4b-41a7-8eb2-744841178857"), null, 200, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Báo cáo khoa học - WoS - Quy đổi giờ nghĩa vụ", new Guid("340bd6e7-9d49-4650-a4cf-f1928358aa7c"), null, new Guid("f81c134b-fd83-4e25-9590-cf7ecfc5b203"), new Guid("03412ca7-8ccf-4903-9018-457768060ab4") },
                    { new Guid("ce72ef2c-a629-40ea-9bab-d77b87421fdf"), null, 34, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học - Quốc tế - Quy đổi vượt định mức - Score 0.5", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 3, new Guid("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("d473791e-fb18-409a-be03-a0b60c75912c"), null, 54, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học - Bộ/Ngành - Quy đổi vượt định mức - Score 0.75", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 2, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("decaf6cd-fb66-440c-9cc3-155036dfc775"), null, 76, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, null, "Bài báo khoa học - Bộ/Ngành - Quy đổi vượt định mức - Score 1", new Guid("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"), 1, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("e4f74734-e0f5-446d-8221-cc3a519ad461"), null, 400, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 100", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 7, new Guid("34f94668-7151-457d-aa06-4bf4e2b27df3"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("f56495e4-eac2-47f5-b282-d3f39055fecb"), null, 160, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Bài báo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ - Score 1", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 1, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") },
                    { new Guid("fcd3b42b-7151-4100-8f08-0019c14a764c"), null, 80, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "Bài báo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ - Score 0.5", new Guid("34fe4df6-0a28-4ddf-930f-19e5febebdee"), 3, new Guid("2d8e237a-bdb3-4d8c-b20a-860f23f65627"), new Guid("2732c858-77dc-471d-bd9a-464a3142530a") }
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
                name: "IX_AuthorRoles_WorkTypeId",
                table: "AuthorRoles",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AuthorRoleId",
                table: "Authors",
                column: "AuthorRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_PurposeId",
                table: "Authors",
                column: "PurposeId");

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
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_FieldId",
                table: "Users",
                column: "FieldId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLevels_WorkTypeId",
                table: "WorkLevels",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_ScoringFieldId",
                table: "Works",
                column: "ScoringFieldId");

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
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "SCImagoFields");

            migrationBuilder.DropTable(
                name: "SystemConfigs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "AuthorRoles");

            migrationBuilder.DropTable(
                name: "Purposes");

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
