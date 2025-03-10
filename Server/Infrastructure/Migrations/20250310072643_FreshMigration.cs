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
                    WorkLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurposeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: false),
                    ConvertHour = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
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
                    WorkLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SCImagoFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScoringFieldId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    DeclaredScore = table.Column<float>(type: "real", nullable: false),
                    FinalAuthorHour = table.Column<int>(type: "int", nullable: false),
                    TempAuthorHour = table.Column<int>(type: "int", nullable: false),
                    TempWorkHour = table.Column<int>(type: "int", nullable: false),
                    IsNotMatch = table.Column<bool>(type: "bit", nullable: false),
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
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authors_WorkId",
                table: "Authors",
                column: "WorkId");

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
                name: "AuthorRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Works");

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
