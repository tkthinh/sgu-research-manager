using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Seeding
{

    public class FieldSeeding : IEntityTypeConfiguration<Field>
    {
        public void Configure(EntityTypeBuilder<Field> builder)
        {
            var fields = new List<Field>
                {
                    new Field { Id = Guid.Parse("222af233-e26e-4e98-a509-4bafa2657512"), Name = "Chăn nuôi-Thú y-Thủy sản" },
                    new Field { Id = Guid.Parse("67d39e1d-7fda-4e2b-8e7a-36b945028cd1"), Name = "Cơ học" },
                    new Field { Id = Guid.Parse("727f23c6-3360-4b0c-95b8-67559f95d696"), Name = "Cơ khí - Động lực" },
                    new Field { Id = Guid.Parse("319bdb13-baa0-41d4-b5a0-77b863f67492"), Name = "Công nghệ Thông tin" },
                    new Field { Id = Guid.Parse("8d67fae5-ca5f-4630-b581-f93979d7f5ab"), Name = "Dược học" },
                    new Field { Id = Guid.Parse("7121be55-10ff-4976-ae97-ee4cb2e098eb"), Name = "Điện - Điện tử - Tự động hóa" },
                    new Field { Id = Guid.Parse("86cc7498-2924-436a-9250-0f379de279d7"), Name = "Giao thông Vận tải" },
                    new Field { Id = Guid.Parse("683650b5-c78c-4dad-adb7-6c49e67340c5"), Name = "Giáo dục học" },
                    new Field { Id = Guid.Parse("e79de642-e149-4617-8cc0-f6b633b6f5d3"), Name = "Hóa học - Công nghệ thực phẩm" },
                    new Field { Id = Guid.Parse("fcf07bae-9441-44e3-ac81-941eaa8f9762"), Name = "Khoa học An ninh" },
                    new Field { Id = Guid.Parse("84670d70-8104-4f36-ab32-cd366dfab481"), Name = "Khoa học Quân sự" },
                    new Field { Id = Guid.Parse("51fe6d6d-f5c5-4992-a7a6-5572dd22562f"), Name = "Khoa học Trái đất - Mỏ" },
                    new Field { Id = Guid.Parse("feff8dba-4647-4577-b766-fe5c9f9b68a4"), Name = "Kinh tế" },
                    new Field { Id = Guid.Parse("32edf4f3-01f0-4531-a51a-4962b11e8f59"), Name = "Luật học" },
                    new Field { Id = Guid.Parse("db6184ab-8bd3-42e1-a346-a69826e877e2"), Name = "Luyện kim" },
                    new Field { Id = Guid.Parse("2a2bbf63-f769-4137-8eaf-72a8519dab42"), Name = "Ngôn ngữ học" },
                    new Field { Id = Guid.Parse("8fcfef89-1f4d-45a9-9062-1f0b2a6dec2c"), Name = "Nông nghiệp - Lâm nghiệp" },
                    new Field { Id = Guid.Parse("8e3a899a-7060-4280-abb7-4fbadc429fd7"), Name = "Sinh học" },
                    new Field { Id = Guid.Parse("0ace7c36-6ac9-4d03-8182-132632a7ff4b"), Name = "Sử học - Khảo cổ học - Dân tộc học" },
                    new Field { Id = Guid.Parse("cefed5af-0f75-4695-8f42-485caa1d9807"), Name = "Tâm lý học" },
                    new Field { Id = Guid.Parse("7fcac010-dc68-4a9e-9244-4f08af9f5fc2"), Name = "Thủy lợi" },
                    new Field { Id = Guid.Parse("5fbfa45d-24e3-40b2-a1e1-12683acb3219"), Name = "Toán học" },
                    new Field { Id = Guid.Parse("549e9619-98e5-4c33-b371-d3eea6866369"), Name = "Triết học - Xã hội học - Chính trị học" },
                    new Field { Id = Guid.Parse("59321fed-e04b-45e0-ac81-a8525a01ba04"), Name = "Văn hóa - Nghệ thuật - Thể dục thể thao" },
                    new Field { Id = Guid.Parse("2b921e8a-8540-4563-946f-de098f1da684"), Name = "Văn học" },
                    new Field { Id = Guid.Parse("04bc6c47-e0f4-4176-b047-11a014d20270"), Name = "Vật lý" },
                    new Field { Id = Guid.Parse("0d79b368-467e-4967-b89f-87e439ba92a6"), Name = "Xây dựng - Kiến trúc" },
                    new Field { Id = Guid.Parse("baf4bd38-28de-407f-8eb0-44e255eac3b9"), Name = "Y học" }
                };

            builder.HasData(fields);
        }
    }

    public class DepartmentSeeding : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            var departments = new List<Department>
                {
                    new Department { Id = Guid.Parse("34d1324e-0f93-4483-83f9-ff0498482555"), Name = "KHOA CÔNG NGHỆ THÔNG TIN" },
                    new Department { Id = Guid.Parse("bb814b70-df6d-4584-b415-a009230eb3fa"), Name = "KHOA ĐIỆN TỬ VIỄN THÔNG" },
                    new Department { Id = Guid.Parse("cb34108f-043b-4e9f-9568-498f514b3513"), Name = "KHOA GIÁO DỤC" },
                    new Department { Id = Guid.Parse("8cb4057f-7108-44a6-9919-47c2d0669fb7"), Name = "KHOA GIÁO DỤC CHÍNH TRỊ" },
                    new Department { Id = Guid.Parse("131d4f64-8e8e-489d-bdd2-36c6920c20bc"), Name = "KHOA GIÁO DỤC MẦM NON" },
                    new Department { Id = Guid.Parse("3dec4757-43b9-41e0-92d3-13c2268e5a9f"), Name = "KHOA GIÁO DỤC QUỐC PHÒNG - AN NINH VÀ GIÁO DỤC THỂ CHẤT" },
                    new Department { Id = Guid.Parse("63391c53-a2cf-4f06-90c6-ead72706aaa9"), Name = "KHOA GIÁO DỤC TIỂU HỌC" },
                    new Department { Id = Guid.Parse("7c29b811-4fe7-42d4-a01c-31a48c0c55b8"), Name = "KHOA MÔI TRƯỜNG" },
                    new Department { Id = Guid.Parse("54ad6a59-caf0-425d-9d0e-24eb62713098"), Name = "KHOA LUẬT" },
                    new Department { Id = Guid.Parse("2f86a581-153a-4b49-ae48-997347feb634"), Name = "KHOA NGOẠI NGỮ" },
                    new Department { Id = Guid.Parse("eb434be4-a7dc-4a13-8eb0-86ab8c01212b"), Name = "KHOA NGHỆ THUẬT" },
                    new Department { Id = Guid.Parse("90237856-e82e-48c6-b802-edbe4d467cde"), Name = "KHOA VĂN HÓA VÀ DU LỊCH" },
                    new Department { Id = Guid.Parse("df145d0b-3b4f-4b72-b35b-dbba6e377522"), Name = "KHOA QUẢN TRỊ KINH DOANH" },
                    new Department { Id = Guid.Parse("94274de8-d2e8-4f3d-9c5c-6941b8c3c604"), Name = "KHOA SƯ PHẠM KHOA HỌC TỰ NHIÊN" },
                    new Department { Id = Guid.Parse("83751766-bcee-4005-bb14-91767f26fdee"), Name = "KHOA SƯ PHẠM KHOA HỌC XÃ HỘI" },
                    new Department { Id = Guid.Parse("3e7e47fd-1c04-4641-8beb-10b50b85e209"), Name = "KHOA TÀI CHÍNH KẾ TOÁN" },
                    new Department { Id = Guid.Parse("b70e6f82-0460-448a-b8a4-7f816db5d0fd"), Name = "KHOA TOÁN - ỨNG DỤNG" },
                    new Department { Id = Guid.Parse("ea5be169-45fd-4528-93dc-a53d83f5a1fb"), Name = "KHOA THƯ VIỆN VĂN PHÒNG" },
                    new Department { Id = Guid.Parse("2b86577e-5842-4021-bae7-793a1d4d920b"), Name = "PHÒNG ĐÀO TẠO" },
                    new Department { Id = Guid.Parse("d32344a0-f267-4ea8-8c24-d3caab71b8aa"), Name = "PHÒNG ĐÀO TẠO SAU ĐẠI HỌC" },
                    new Department { Id = Guid.Parse("6fc3ffbd-bc13-4d89-88d9-d0420771461d"), Name = "PHÒNG GIÁO DỤC THƯỜNG XUYÊN" },
                    new Department { Id = Guid.Parse("cd88a07d-cb87-4354-8f41-7bdc557b144e"), Name = "PHÒNG QUẢN LÝ KHOA HỌC" },
                    new Department { Id = Guid.Parse("56392386-966f-4366-9769-864a1021b53d"), Name = "PHÒNG KHẢO THÍ VÀ ĐẢM BẢO CHẤT LƯỢNG GIÁO DỤC" },
                    new Department { Id = Guid.Parse("e7ba9cbe-63ed-4efe-a50d-43640b74c92f"), Name = "PHÒNG KẾ HOẠCH - TÀI CHÍNH" },
                    new Department { Id = Guid.Parse("334d3f98-43b1-4dbb-9809-eff2c70f0441"), Name = "VĂN PHÒNG" },
                    new Department { Id = Guid.Parse("3f39da19-a532-4759-abab-aad4bd56a3f8"), Name = "TRẠM Y TẾ" }
                };

            builder.HasData(departments);
        }
    }

    public class WorkTypeSeeding : IEntityTypeConfiguration<WorkType>
    {
        public void Configure(EntityTypeBuilder<WorkType> builder)
        {
            var workTypes = new List<WorkType>
                {
                    new WorkType { Id = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"), Name = "Bài báo khoa học" },
                    new WorkType { Id = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"), Name = "Báo cáo khoa học" },
                    new WorkType { Id = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"), Name = "Đề tài" },
                    new WorkType { Id = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b"), Name = "Tài liệu giảng dạy" },
                    new WorkType { Id = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"), Name = "Giáo trình" },
                    new WorkType { Id = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"), Name = "Hội thảo, hội nghị" },
                    new WorkType { Id = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"), Name = "Hướng dẫn SV NCKH" },
                    new WorkType { Id = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"), Name = "Khác" },
                    new WorkType { Id = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97"), Name = "Chương sách" },
                    new WorkType { Id = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38"), Name = "Chuyên khảo" },
                    new WorkType { Id = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9"), Name = "Tham khảo" },
                    new WorkType { Id = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256"), Name = "Giáo trình - Sách" },
                    new WorkType { Id = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0"), Name = "Tài liệu hướng dẫn" }
                };

            builder.HasData(workTypes);
        }
    }

    public class WorkLevelSeeding : IEntityTypeConfiguration<WorkLevel>
    {
        public void Configure(EntityTypeBuilder<WorkLevel> builder)
        {
            var workLevels = new List<WorkLevel>
                {
                    // Đề tài
                    //new WorkLevel {
                    //    Id = Guid.Parse("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"),
                    //    Name = "Khoa",
                    //    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    //},
                    new WorkLevel {
                        Id = Guid.Parse("b386e9ba-8844-42eb-b910-6cb360c5485b"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"),
                        Name = "Bộ/Ngành",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("98c20000-d8e8-4325-93d4-c2d238ac2151"),
                        Name = "Tỉnh/Thành phố",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("a210b965-4e0d-41be-a84d-4480bea000f1"),
                        Name = "Nhà nước",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("b2581ebc-a310-460b-9721-f88c92ed2c81"),
                        Name = "Quốc tế",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },

                    // Tài liệu giảng dạy
                    new WorkLevel {
                        Id = Guid.Parse("c6846c10-1297-40c7-820a-76cc286fce31"),
                        Name = "Khoa",
                        WorkTypeId = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b")
                    },

                    // Giáo trình
                    new WorkLevel {
                        Id = Guid.Parse("483f26c2-8218-4d4b-a374-1fbd3a4fc250"),
                        Name = "Khoa",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("0e011f57-5ff7-476f-b2bc-46243468fdcb"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("d588e361-97a2-44cf-a507-24255430dbe7"),
                        Name = "Bộ/Ngành",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("c81240d2-dd87-4949-8252-0116cb5a0cc8"),
                        Name = "Tỉnh/Thành phố",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("e0264c17-7865-4e6d-b707-6e5227bc63d1"),
                        Name = "Nhà nước",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("057a8b2a-7283-43f9-926d-838c7be46987"),
                        Name = "Quốc tế",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },

                    // Bài báo khoa học
                    new WorkLevel {
                        Id = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                        Name = "Bộ/Ngành",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                        Name = "Quốc tế",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                        Name = "WoS",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                        Name = "Scopus",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },

                    // Báo cáo khoa học
                    new WorkLevel {
                        Id = Guid.Parse("3c21b247-16ce-40a9-a921-abef0e1bba56"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),
                        Name = "Bộ/Ngành",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),
                        Name = "Quốc tế",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),
                        Name = "Quốc gia",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),
                        Name = "WoS",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),
                        Name = "Scopus",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },

                    // Hội thảo, hội nghị
                    new WorkLevel {
                        Id = Guid.Parse("071464ae-332b-4426-9b03-cbdd05c2d5bc"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"),
                        Name = "Quốc gia",
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("bec79373-6f38-4f53-ba87-e986b83ce3b2"),
                        Name = "Quốc tế",
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },

                    // Hướng dẫn sinh viên NCKH
                    new WorkLevel {
                        Id = Guid.Parse("69cc26ee-f6b8-46a6-9229-e42219775d78"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("6bbf7e31-bcca-4078-b894-7c8d3afba607"),
                        Name = "Bộ",
                        WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("08becbaf-2a92-4de1-8908-454c4659ad94"),
                        Name = "Eureka",
                        WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f")
                    },

                    // Khác
                    new WorkLevel {
                        Id = Guid.Parse("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"),
                        Name = "Trường",
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"),
                        Name = "Tỉnh/Thành phố",
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                        Name = "Quốc gia",
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },
                    new WorkLevel {
                        Id = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                        Name = "Quốc tế",
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },
                };

            builder.HasData(workLevels);
        }
    }

    public class AuthorRoleSeeding : IEntityTypeConfiguration<AuthorRole>
    {
        public void Configure(EntityTypeBuilder<AuthorRole> builder)
        {
            var authorRoles = new List<AuthorRole>
                {
                    // Bài báo khoa học
                    new AuthorRole {
                        Id = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),
                        Name = "Tác giả chính",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },

                    // Báo cáo khoa học
                    new AuthorRole {
                        Id = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),
                        Name = "Tác giả chính",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },

                    // Khác
                    new AuthorRole {
                        Id = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                        Name = "Tác giả chính",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },

                    // Chương sách
                    new AuthorRole {
                        Id = Guid.Parse("6304f87f-439a-477d-b989-31df3b6e06b6"),
                        Name = "Chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("3f0d8b5e-99da-4702-bc34-1b36c99cbdaa"),
                        Name = "Đồng chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("98b05ce5-af6e-4953-be9b-45f97e711c86"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97")
                    },

                    // Chuyên khảo
                    new AuthorRole {
                        Id = Guid.Parse("5f65dffc-5e3a-46a8-9bc6-1bacce9ef3fa"),
                        Name = "Chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("4a85d698-7809-4912-923f-18c3f0a2e676"),
                        Name = "Đồng chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("3dfd761c-256e-442f-99fb-136d27b4cea5"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38")
                    },

                    // Tham khảo
                    new AuthorRole {
                        Id = Guid.Parse("11eea600-4495-486c-985d-57de08b8b5da"),
                        Name = "Chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("77daab84-939d-4d0d-957d-27be75bb79b4"),
                        Name = "Đồng chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("b0923868-3ce3-4653-97fa-d6925771ce64"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9")
                    },

                    // Hướng dẫn SV NCKH
                    new AuthorRole {
                        Id = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                        Name = "GV hướng dẫn",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f")
                    },

                    // Giáo trình - Sách
                    new AuthorRole {
                        Id = Guid.Parse("8560f2b2-7b9b-4f28-b79a-f5ea21f76e97"),
                        Name = "Chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("be6b03da-7853-48ab-93b3-81da27c3271e"),
                        Name = "Đồng chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("8bac0cd7-b553-42cc-af1a-5d50d32a6fac"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256")
                    },

                    // Tài liệu hướng dẫn
                    new AuthorRole {
                        Id = Guid.Parse("d8d1af53-3354-4af3-a18f-85c6ee46e750"),
                        Name = "Chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("ed76e468-43ee-47c3-8148-e6f63406a98d"),
                        Name = "Đồng chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("3016bf69-e8d1-4852-a717-b5924a7bb7b2"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0")
                    },

                    // Đề tài
                    new AuthorRole {
                        Id = Guid.Parse("e51ba448-a481-4d5e-a560-4b81c45a0530"),
                        Name = "Chủ nhiệm",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },

                    // Tài liệu giảng dạy
                    new AuthorRole {
                        Id = Guid.Parse("efb42251-5b12-4ac2-b291-e6f8ebd716d4"),
                        Name = "Chủ nhiệm",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("e79d70b3-ea7c-43d8-b441-5317c975ddb3"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b")
                    },
                    // Giáo trình
                    new AuthorRole {
                        Id = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),
                        Name = "Chủ biên",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),
                        Name = "Thành viên",
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },

                    // Hội thảo, hội nghị
                    new AuthorRole {
                        Id = Guid.Parse("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"),
                        Name = "Trưởng ban",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("822d8f31-2b1d-4367-8c50-e4535fac5b5f"),
                        Name = "Phó trưởng ban",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("ad3aa473-c140-46cb-b8f4-faecdf2f338e"),
                        Name = "Ủy viên thường trực",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"),
                        Name = "Ban Chuyên môn",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"),
                        Name = "Ban Biên tập kỹ yếu",
                        IsMainAuthor = true,
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    }
                };

            builder.HasData(authorRoles);
        }
    }

    public class PurposeSeeding : IEntityTypeConfiguration<Purpose>
    {
        public void Configure(EntityTypeBuilder<Purpose> builder)
        {
            var purposes = new List<Purpose>
                {
                    // Bài báo khoa học
                    new Purpose {
                        Id = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new Purpose {
                        Id = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                        Name = "Quy đổi vượt định mức",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },
                    new Purpose {
                        Id = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),
                        Name = "Sản phẩm của đề tài NCKH",
                        WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a")
                    },

                    // Báo cáo khoa học
                    new Purpose {
                        Id = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new Purpose {
                        Id = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),
                        Name = "Quy đổi vượt định mức",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },
                    new Purpose {
                        Id = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),
                        Name = "Sản phẩm của đề tài NCKH",
                        WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4")
                    },

                    // Đề tài
                    new Purpose {
                        Id = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },

                    // Tài liệu giảng dạy
                    new Purpose {
                        Id = Guid.Parse("63db31b6-8f8f-4a23-8069-331708fe8ebe"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b")
                    },

                    // Giáo trình
                    new Purpose {
                        Id = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d")
                    },

                    // Hội thảo, hội nghị
                    new Purpose {
                        Id = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },

                    // Hướng dẫn SV NCKH
                    new Purpose {
                        Id = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f")
                    },

                    // Khác
                    new Purpose {
                        Id = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c")
                    },

                    // Chương sách
                    new Purpose {
                        Id = Guid.Parse("494e049e-0972-4ff0-a786-6e00880955fc"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97")
                    },

                    // Chuyên khảo
                    new Purpose {
                        Id = Guid.Parse("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38")
                    },

                    // Tham khảo
                    new Purpose {
                        Id = Guid.Parse("3da2c117-b32f-4687-89b8-ba9544920f35"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9")
                    },

                    // Giáo trình - Sách
                    new Purpose {
                        Id = Guid.Parse("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256")
                    },

                    // Tài liệu hướng dẫn
                    new Purpose {
                        Id = Guid.Parse("fc948f99-b569-4265-b1c9-ba5aa31d730b"),
                        Name = "Quy đổi giờ nghĩa vụ",
                        WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0")
                    }
                };

            builder.HasData(purposes);
        }
    }

    public class FactorSeeding : IEntityTypeConfiguration<Factor>
    {
        public void Configure(EntityTypeBuilder<Factor> builder)
        {
            var factors = new List<Factor>
            {
                // Bài báo khoa học 
                new Factor
                {
                    Id = Guid.Parse("efce39b7-5723-41e7-acd2-0e7bd5b66d1d"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },

                new Factor
                {
                    Id = Guid.Parse("52e2bd00-fcbb-40a4-b059-4db425c61202"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("ad3bdf4e-4697-43bd-a20c-d7ab63e6e59e"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 800
                },

                new Factor
                {
                    Id = Guid.Parse("258a7107-baf3-4632-96be-ada15af33184"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 800
                },
                new Factor
                {
                    Id = Guid.Parse("c99d0edf-12db-45f4-bf9f-722074384101"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 600
                },
                new Factor
                {
                    Id = Guid.Parse("d8f80823-6534-47c1-abaf-2ddeb4c4590b"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 600
                },
                new Factor
                {
                    Id = Guid.Parse("4b04f15e-297a-47a8-a573-28615f19f042"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 640
                },
                new Factor
                {
                    Id = Guid.Parse("a593c6f6-b82a-4415-bae9-309524e6f01e"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 640
                },
                new Factor
                {
                    Id = Guid.Parse("18f442a7-0df2-4579-ad53-afe90aedf9b3"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 400
                },
                new Factor
                {
                    Id = Guid.Parse("47ac6cea-5dfd-4f40-bb94-7be183fa9421"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 400
                },
                new Factor
                {
                    Id = Guid.Parse("9e6cf226-ce18-45f4-86e7-04cded49b021"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("07153ca2-a9cd-44c6-9efa-fba5e65a046f"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("34769b69-b8b3-4bbd-a0a8-c789c333c134"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("54064ab2-4172-4331-9c9f-68a7a384300a"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("c0f3763d-38c5-4016-aef7-9904d77841f0"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("5a49b577-eb55-4729-a479-0d855d4ce2bd"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("b51b05e2-774d-476b-b26f-ebf8fc3b90a1"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("cc99e5e9-e74d-4b21-8603-6e93ddc0c56c"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("df7fd664-6298-4acf-9094-d0212bf923b6"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản  phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("ac12bd71-e645-485d-9f57-6d2efb931657"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản  phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("3f661984-45ac-45af-b665-d7c8f609d172"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 800
                },
                new Factor
                {
                    Id = Guid.Parse("b611282d-b594-4e52-b2c5-058911f8a0fb"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 800
                },
                new Factor
                {
                    Id = Guid.Parse("d1ddaeeb-532a-4037-9586-05af4446a390"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 600
                },
                new Factor
                {
                    Id = Guid.Parse("b1139db5-197e-44a9-a5a1-418a01d36e53"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 10% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopMuoi,
                    MaxAllowed = null,
                    ConvertHour = 600
                },

                new Factor
                {
                    Id = Guid.Parse("5e2924fb-9268-4ab0-8527-60096dd3d063"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 640
                },
                new Factor
                {
                    Id = Guid.Parse("9632d54d-3a34-4a39-a388-0550a9ca4733"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 640
                },
                new Factor
                {
                    Id = Guid.Parse("ee76f503-a342-4699-a320-cd53a4767322"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 400
                },
                new Factor
                {
                    Id = Guid.Parse("621a47fe-6337-4cf1-a79e-d997627dc1ee"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp Scopus
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 30% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopBaMuoi,
                    MaxAllowed = null,
                    ConvertHour = 400
                },

                new Factor
                {
                    Id = Guid.Parse("56f1596d-64c5-437d-a22b-853266bbeb93"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("b2f41bf5-c1f0-488c-872a-82d3f0e06fae"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("f029e7f1-69eb-4043-94f7-c9657b383793"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("750d273d-b462-4e5c-87cb-266e7a8d201d"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top 50% tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopNamMuoi,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("d1028c26-d4c5-4643-91d1-50824a7ba47e"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 560
                },
                new Factor
                {
                    Id = Guid.Parse("65309514-86ac-4ca2-a957-67616a478f0d"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("35e25540-6ee6-49a1-8188-39b5d8beaa13"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),   // Cấp WoS
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học thuộc top còn lại tạp chí hàng đầu",
                    ScoreLevel = ScoreLevel.BaiBaoTopConLai,
                    MaxAllowed = 8,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("135f96eb-8ab0-494c-ac9a-c115b470c3d6"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("4eb5955a-db99-4a23-99a7-50a94d05ce3c"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("a575e613-c9ab-44b5-8eb6-33f8914c36d4"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản  phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("89760e99-da91-46ef-bda4-976f3c6572b0"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản  phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("b18b9a28-d43d-4258-a3f5-14939ee182ef"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản  phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("a47f4c75-e5b0-4111-b9d2-beb250aa36a9"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("173671ca-9de0-4c91-84bb-ffe7dec887e5"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("6ede2fa2-0b00-4917-91a7-66bea34b8a9a"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("d643e931-18d9-4436-a3f6-0e0cb8fc5d38"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("22505328-67be-49d6-9869-7431bd91f663"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("5cf30509-8632-4d62-ad14-55949b9b9336"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("fff69169-c768-4d01-b7d0-360b98899173"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("d30ad887-019a-4023-955c-bba587b424d7"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("47876171-baff-47c7-ba03-ab35ed7502b0"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("80957f93-d4b1-431b-8287-4c3d55e9d26c"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("10bc8772-db03-494a-8564-3ddb8c80af4e"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },

                new Factor
                {
                    Id = Guid.Parse("90b005be-d7fe-4e6a-acbc-157910884c08"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Trường được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("722f0278-63c0-4488-973f-dc8baa2729e8"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("e88c0e99-9eb0-4218-8310-7d7a95886634"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 1.0 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoMotDiem,
                    MaxAllowed = 4,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("0f517009-94d4-488a-94d3-1d57bfd21964"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("695a0787-7f2f-48b9-916e-3d1a5f0698c5"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("d9fc76b8-c3bf-4944-b625-971762b1dff6"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("166958e9-3e92-40d2-b7e6-f6cf019cbec0"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("05644135-f75c-4d65-8c0f-2382c0533880"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("918bb01d-f02b-4c07-afda-fbb115b2533d"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("13610180-0d84-47c8-a280-1d342d52001b"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("5a94c31b-93bb-43c3-ad13-f7cfc9c8d702"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("6482dbfa-e158-470b-ba51-d6322e7b9684"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("0286a29f-f60d-4ded-ad7e-0f145290f36a"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("3a624e20-75c6-47f5-96e5-281ce6c63eaa"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("62c97efa-acb4-4e13-9887-881d79a57892"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.75 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoKhongBayNamDiem,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("2f7c8dd0-91e5-4b5d-b1d0-fcd1be18bc54"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("5590c2f4-733d-4482-85a0-12ed2b76a560"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("61ea9a32-4b56-452d-91ec-cf54ba2ee568"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },

                new Factor
                {
                    Id = Guid.Parse("bec1ee6d-0f3d-4d7c-b868-66b130d1c60b"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("c17fe0b3-0eb8-456c-9e75-ef441bd458c6"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("6dd9f77e-a652-4096-8523-71b8f6a4a389"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },

                new Factor
                {
                    Id = Guid.Parse("daff3e2a-5f02-435c-81a7-f4794ce32259"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 34
                },
                new Factor
                {
                    Id = Guid.Parse("73e5510c-66fa-45db-8c3c-245f3bc2b860"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 34
                },
                new Factor
                {
                    Id = Guid.Parse("6b0be41d-b561-40f8-92cc-0c4957f3fc7c"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("c20d5d29-cf3e-40c5-be56-a2798511c3bc"),  // Thành viên
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 34
                },

                new Factor
                {
                    Id = Guid.Parse("ca590e0d-797b-46d4-87b7-a223e390b02c"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),   // Cấp Trường
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Trường được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 34
                },
                new Factor
                {
                    Id = Guid.Parse("d9d00031-6a8d-4939-bc95-25a747ceeaec"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Bộ/Ngành được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 34
                },
                new Factor
                {
                    Id = Guid.Parse("2d69276a-cac9-49eb-8623-f5b55c5691d1"),
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),    // Bài báo
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("069b5046-0a7e-47d9-a8f0-af09db20a697"),  // Tác giả chính
                    Name = "Bài báo khoa học cấp Quốc tế được tính đến 0.5 điểm theo Danh mục tạp chí khoa học",
                    ScoreLevel = ScoreLevel.BaiBaoNuaDiem,
                    MaxAllowed = 2,
                    ConvertHour = 34
                },

                // Báo cáo khoa học
                new Factor
                {
                    Id = Guid.Parse("8a94c350-df68-4f57-b57f-ae6dd7a0a3dc"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("3c21b247-16ce-40a9-a921-abef0e1bba56"),   // Cấp Trường
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Trường được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 40
                },
                new Factor
                {
                    Id = Guid.Parse("0570292e-3849-4a0e-b66f-32fa34a97f48"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("3c21b247-16ce-40a9-a921-abef0e1bba56"),   // Cấp Trường
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Trường được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 40
                },

                new Factor
                {
                    Id = Guid.Parse("aa95d339-ea61-4442-8975-533a7c8432d2"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("3c21b247-16ce-40a9-a921-abef0e1bba56"),   // Cấp Trường
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Trường được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("05a85289-1c41-419f-b8c8-a680833a532d"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("3c21b247-16ce-40a9-a921-abef0e1bba56"),   // Cấp Trường
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Trường được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("9007980a-5ba4-497f-a7f7-ecee5310c146"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("e78fab66-8637-431d-a2f8-a8d3d81f7124"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("371136aa-c37c-4f80-88b9-916273b3484d"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Quốc gia được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("83d5e873-4216-4e5e-969d-88cb7aa596b9"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Quốc gia được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("65a1f78b-0de9-415e-91e9-40c03fb25275"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Quốc tế được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("85b73b4f-079c-4e77-b04b-9f4c0c913d79"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Quốc tế được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                }, 
                new Factor
                {
                    Id = Guid.Parse("e2ad03e5-11d4-42e4-9626-49db6350bf9a"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),   // Cấp Scopus
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Scopus được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("2c5a2f69-f5c6-493e-968b-3e844154ea0b"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),   // Cấp Scopus
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Scopus được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("d203dfe4-b44a-4c37-a17b-726fbacbdd47"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),   // Cấp WoS
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp WoS được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("e5105b69-738c-493d-8a8d-a2a10f282546"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),   // Cấp WoS
                    PurposeId = Guid.Parse("be2cb497-02ac-4f5c-ae8a-062876730a2b"),     // Sản phẩm thuộc đề tài
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp WoS được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 0
                },
                new Factor
                {
                    Id = Guid.Parse("1d749956-707f-4e55-ae63-7e8ad787d716"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 1,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("417cb0bf-ef69-423f-8fa4-3fd4bf3109ad"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 1,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("83cadff5-b7df-4745-81f8-2ee792b3e9b0"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 1,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("1a015081-cc66-48c4-b6eb-13ffe4aeb756"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Bộ/Ngành được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 1,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("d70faba4-e61e-4dae-b683-2e46e73d4578"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Quốc gia được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("bae86fa5-c497-4074-8439-db7b54ae6455"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Quốc gia được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("7059bcf8-0d4e-4e9b-840f-33ea4350972c"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Quốc gia được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("e19e5af4-d301-4881-949b-fa595d175b97"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Quốc gia được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 54
                },
                new Factor
                {
                    Id = Guid.Parse("9af34412-349c-4cad-a50f-0bf64bdf9eca"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Quốc tế được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("a523fa26-ee55-4576-92a8-35a7ed52f70e"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Quốc tế được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("d20c4ce0-a360-4f96-889a-49ed20ce0156"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Quốc tế được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 76
                },
                new Factor
                {
                    Id = Guid.Parse("0d943cc8-f735-4b81-9038-7dfdac675d88"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Quốc tế được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 76
                },
                new Factor
                {
                    Id = Guid.Parse("7bebd5b0-0101-420a-b6ac-5b1cdbce04ff"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),   // Cấp WoS
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp WoS được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 200
                },
                new Factor
                {
                    Id = Guid.Parse("d4655ab5-d317-4056-a71c-d2d72b2580db"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),   // Cấp WoS
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp WoS được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 200
                },
                new Factor
                {
                    Id = Guid.Parse("698099c7-4e76-41e4-a65c-98d07bd7da17"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),   // Cấp WoS
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp WoS được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("457f5822-625c-4ce2-81db-c5c5cc99d0ca"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),   // Cấp WoS
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp WoS được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("957e6f12-5644-4b54-ac9f-b582f7b11d7b"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),   // Cấp Scopus
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Scopus được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 200
                },
                new Factor
                {
                    Id = Guid.Parse("2bd93368-b474-44fc-8146-0bf872f6bb80"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),   // Cấp Scopus
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Scopus được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 200
                },

                new Factor
                {
                    Id = Guid.Parse("57b568a1-5447-4bb8-bc39-7bc870f86560"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),   // Cấp Scopus
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("b92d7fc9-687d-4fdd-9ddf-c4c7b50ae4c5"),  // Tác giả chính
                    Name = "Báo cáo khoa học cấp Scopus được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("d3c389cc-db9d-4112-878e-48408364b467"),
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),    // Báo cáo
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),   // Cấp Scopus
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),     // Giờ vượt định mức
                    AuthorRoleId = Guid.Parse("ee9dc844-73d7-458f-9f6d-ae535824c8ca"),  // Thành viên
                    Name = "Báo cáo khoa học cấp Scopus được đăng toàn văn",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 120
                },



                // Tài liệu giảng dạy
                new Factor
                {
                    Id = Guid.Parse("ec1679cd-c303-4545-a774-82bbdbff81c0"),
                    WorkTypeId = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b"),    // Tài liệu giảng dạy
                    WorkLevelId = Guid.Parse("c6846c10-1297-40c7-820a-76cc286fce31"),   // Cấp Khoa
                    PurposeId = Guid.Parse("63db31b6-8f8f-4a23-8069-331708fe8ebe"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("efb42251-5b12-4ac2-b291-e6f8ebd716d4"),  // Chủ nhiệm
                    Name = "Tài liệu giảng dạy cấp khoa",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("a1e70b5f-1670-4254-b0a9-6b3c3936356e"),
                    WorkTypeId = Guid.Parse("9787de81-78f2-4797-b810-0f5ec411125b"),    // Tài liệu giảng dạy
                    WorkLevelId = Guid.Parse("c6846c10-1297-40c7-820a-76cc286fce31"),   // Cấp Khoa
                    PurposeId = Guid.Parse("63db31b6-8f8f-4a23-8069-331708fe8ebe"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("e79d70b3-ea7c-43d8-b441-5317c975ddb3"),  // Thành viên
                    Name = "Tài liệu giảng dạy cấp khoa",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 80
                },

                // Đề tài
                new Factor
                {
                    Id = Guid.Parse("455360e6-693a-47e9-8671-8a83393149ad"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("b386e9ba-8844-42eb-b910-6cb360c5485b"),   // Cấp Trường
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("e51ba448-a481-4d5e-a560-4b81c45a0530"),  // Chủ nhiệm
                    Name = "Đề tài nghiên cứu cấp trường",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("a80a0cd2-2bab-4e32-8cf3-a56d5a33cacc"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("b386e9ba-8844-42eb-b910-6cb360c5485b"),   // Cấp Trường
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"),  // Thành viên
                    Name = "Đề tài nghiên cứu cấp trường",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("4f68bc47-df55-4fa5-80e7-457e984f4850"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("a210b965-4e0d-41be-a84d-4480bea000f1"),   // Cấp Nhà nước
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("e51ba448-a481-4d5e-a560-4b81c45a0530"),  // Chủ nhiệm
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("877b45f8-db8c-4148-84f1-565319312ca2"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("a210b965-4e0d-41be-a84d-4480bea000f1"),   // Cấp Nhà nước
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"),  // Thành viên
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("56119049-182c-4e4e-8fe2-f0ee3eade9b7"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("e51ba448-a481-4d5e-a560-4b81c45a0530"),  // Chủ nhiệm
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("81ecbcbc-6b7b-470c-9ae8-ab745c7106fe"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("0485b444-1c9c-4f7f-a576-7cdddd0ca1db"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"),  // Thành viên
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("9d0bcefd-06c5-427c-a967-64c0a11e2326"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("98c20000-d8e8-4325-93d4-c2d238ac2151"),   // Cấp Tỉnh/Thành phố
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("e51ba448-a481-4d5e-a560-4b81c45a0530"),  // Chủ nhiệm
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("7d26c014-1711-459e-8e18-0da00972dd40"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("98c20000-d8e8-4325-93d4-c2d238ac2151"),   // Cấp Tỉnh/Thành phố
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"),  // Thành viên
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("5aac86cc-aa1e-4a24-895e-f5fe61b76f18"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("b2581ebc-a310-460b-9721-f88c92ed2c81"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("e51ba448-a481-4d5e-a560-4b81c45a0530"),  // Chủ nhiệm
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("510e035d-f66a-4489-accd-4c259520c507"),
                    WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348"),    // Đề tài
                    WorkLevelId = Guid.Parse("b2581ebc-a310-460b-9721-f88c92ed2c81"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("b622853d-f917-4871-a3b9-9a1d29ce9506"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("d8a24a90-4f1e-447e-bfe5-958fb9ce231c"),  // Thành viên
                    Name = "Đề tài nghiên cứu cấp Cơ sở",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Giáo trình
                new Factor
                {
                    Id = Guid.Parse("60c6a668-ef9f-4e1f-ad70-737f5f23756c"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("483f26c2-8218-4d4b-a374-1fbd3a4fc250"),   // Cấp Khoa
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),  // Thành viên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("5f4545c3-9f56-4f17-8b74-3ea21825fd50"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("483f26c2-8218-4d4b-a374-1fbd3a4fc250"),   // Cấp Khoa
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),  // Chủ biên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("9333b444-ec14-423b-9cab-4a8facf075f5"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("0e011f57-5ff7-476f-b2bc-46243468fdcb"),   // Cấp Trường
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),  // Thành viên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("7493c3a8-923e-45c5-83e3-45baf67107e2"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("0e011f57-5ff7-476f-b2bc-46243468fdcb"),   // Cấp Trường
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),  // Chủ biên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("fda36ff3-5b58-410a-965c-134ffe46f6ab"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("d588e361-97a2-44cf-a507-24255430dbe7"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),  // Thành viên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("b7464c59-7774-4f77-9bdb-c3ad962d2067"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("d588e361-97a2-44cf-a507-24255430dbe7"),   // Cấp Bộ/Ngành
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),  // Chủ biên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("c991b3fb-3d52-4896-8ea7-429b21b5dbe9"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("c81240d2-dd87-4949-8252-0116cb5a0cc8"),   // Cấp Tỉnh/Thành phố
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),  // Thành viên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("7cc503bc-ad8c-4f97-abe4-7987a115d5d8"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("c81240d2-dd87-4949-8252-0116cb5a0cc8"),   // Cấp Tỉnh/Thành phố
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),  // Chủ biên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("bc3ae1fa-70eb-4385-a14f-7a820de27846"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("e0264c17-7865-4e6d-b707-6e5227bc63d1"),   // Cấp Nhà nước
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),  // Thành viên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("9cde2653-dcc6-4297-91d4-218f0829ae35"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("e0264c17-7865-4e6d-b707-6e5227bc63d1"),   // Cấp Nhà nước
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),  // Chủ biên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("8b8fe062-6735-4c5f-a0e6-d0d061737a89"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("057a8b2a-7283-43f9-926d-838c7be46987"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("6dbe3055-a0af-4ea2-971f-f3dbcfb58370"),  // Thành viên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("902745db-0845-44e7-9f41-f7b44faddb34"),
                    WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d"),    // Giáo trình
                    WorkLevelId = Guid.Parse("057a8b2a-7283-43f9-926d-838c7be46987"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("4511eace-33a7-40eb-b7b8-5570c5ea1cb1"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("1c563e5d-0bc0-4861-8ae0-62835d64daa9"),  // Chủ biên
                    Name = "Nhiệm vụ biên soạn, chỉnh lý giáo trình",
                    ScoreLevel = null,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Hội thảo, hội nghị
                new Factor
                {
                    Id = Guid.Parse("62155dde-e5d3-4497-898d-b9765212fade"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("071464ae-332b-4426-9b03-cbdd05c2d5bc"),   // Cấp Trường
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"),  // Trường ban
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("b52a5fbe-bb11-4026-9d2d-70654e4fefb8"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("071464ae-332b-4426-9b03-cbdd05c2d5bc"),   // Cấp Trường
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("822d8f31-2b1d-4367-8c50-e4535fac5b5f"),  // Phó Trưởng ban
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("7c3cd464-8742-4f93-abff-57073711d1c4"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("071464ae-332b-4426-9b03-cbdd05c2d5bc"),   // Cấp Trường
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ad3aa473-c140-46cb-b8f4-faecdf2f338e"),  // Ủy viên thường trực
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("fe73778c-1757-4ad7-92da-470154b1c01d"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("071464ae-332b-4426-9b03-cbdd05c2d5bc"),   // Cấp Trường
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"),  // Ban chuyên môn
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 10
                },
                new Factor
                {
                    Id = Guid.Parse("f325d698-f4d1-4ab4-915d-f871b509ec92"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("071464ae-332b-4426-9b03-cbdd05c2d5bc"),   // Cấp Trường
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"),  // Ban biên tập kỷ yếu
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 10
                },
                new Factor
                {
                    Id = Guid.Parse("a4d78d2d-7561-4cfd-b042-c7d37f26df39"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"),  // Trưởng ban
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("b15d18ef-55c6-42ba-815b-9d8855a20563"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("822d8f31-2b1d-4367-8c50-e4535fac5b5f"),  // Phó Trưởng ban
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("34cb99bf-5011-401f-b53c-13448c4ab1bf"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ad3aa473-c140-46cb-b8f4-faecdf2f338e"),  // Ủy viên thường trực
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("10b587a5-dff5-4d64-8238-18b45348557d"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"),  // Ban chuyên môn
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 10
                },
                new Factor
                {
                    Id = Guid.Parse("68ecc77b-b90c-49cc-adc7-09e5f1257523"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("d94f2107-409a-4b2a-a5ae-960d7cc6f3a0"),   // Cấp Quốc gia
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"),  // Ban biên tập kỷ yếu
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 10
                },
                new Factor
                {
                    Id = Guid.Parse("fcac02d5-8f7b-4e0e-aa97-c1d63d86fcc6"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("bec79373-6f38-4f53-ba87-e986b83ce3b2"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("4ef8dcc3-7bcc-4ab2-a890-d673546a1089"),  // Trưởng ban
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("8f8a1b3d-3f7e-4fc8-9e8e-9f3f8ead1eaf"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("bec79373-6f38-4f53-ba87-e986b83ce3b2"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("822d8f31-2b1d-4367-8c50-e4535fac5b5f"),  // Phó Trưởng ban
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("b9a61a17-0e15-44a6-b77c-e8804f31bf4b"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("bec79373-6f38-4f53-ba87-e986b83ce3b2"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("ad3aa473-c140-46cb-b8f4-faecdf2f338e"),  // Ủy viên thường trực
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 5
                },
                new Factor
                {
                    Id = Guid.Parse("bbd4e0d2-e4ab-4cf8-b4a3-953142b5efc5"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("bec79373-6f38-4f53-ba87-e986b83ce3b2"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("cd929fdb-3aa2-40dd-97ad-f46392ba1d30"),  // Ban chuyên môn
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 10
                },
                new Factor
                {
                    Id = Guid.Parse("0a45fdcf-34d4-4709-9d55-53e6380c8ccc"),
                    WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa"),    // Hội thảo
                    WorkLevelId = Guid.Parse("bec79373-6f38-4f53-ba87-e986b83ce3b2"),   // Cấp Quốc tế
                    PurposeId = Guid.Parse("f49c3e00-2819-4c03-90ce-b8705555933c"),     // Giờ nghĩa vụ
                    AuthorRoleId = Guid.Parse("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"),  // Ban biên tập kỷ yếu
                    Name = "Tham gia tổ chức Hội thảo khoa học",
                    ScoreLevel = null,
                    MaxAllowed = 2,
                    ConvertHour = 10
                },

                // Hướng dẫn sinh viên NCKH
                new Factor
                {
                    Id = Guid.Parse("b986e0d6-36c8-4b73-ab80-566d519bff16"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("6bbf7e31-bcca-4078-b894-7c8d3afba607"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Khuyến khích",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiKK,
                    MaxAllowed = null,
                    ConvertHour = 40
                },
                new Factor
                {
                    Id = Guid.Parse("166988df-84b4-4b0f-a1e0-8d356a1f4346"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("08becbaf-2a92-4de1-8908-454c4659ad94"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Khuyến khích",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiKK,
                    MaxAllowed = null,
                    ConvertHour = 40
                },
                new Factor
                {
                    Id = Guid.Parse("60772df7-9150-4219-9ee8-ce5439144b0c"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("6bbf7e31-bcca-4078-b894-7c8d3afba607"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Ba",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiBa,
                    MaxAllowed = null,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("1ebfedcf-12c6-408a-82fd-170f9211d0d3"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("08becbaf-2a92-4de1-8908-454c4659ad94"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Ba",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiBa,
                    MaxAllowed = null,
                    ConvertHour = 80
                },

                new Factor
                {
                    Id = Guid.Parse("5aeed66d-b2ae-448f-8e30-f7c005c54ff2"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("6bbf7e31-bcca-4078-b894-7c8d3afba607"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Nhì",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiNhi,
                    MaxAllowed = null,
                    ConvertHour = 120
                },
                new Factor
                {
                    Id = Guid.Parse("dd1ea2c7-4cc5-442d-b8fa-c6a4f8a663a2"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("08becbaf-2a92-4de1-8908-454c4659ad94"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Nhì",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiNhi,
                    MaxAllowed = null,
                    ConvertHour = 120
                },

                new Factor
                {
                    Id = Guid.Parse("aee0c04a-26bc-4ef6-92b7-3d78f6ccaa61"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("6bbf7e31-bcca-4078-b894-7c8d3afba607"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Nhất",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiNhat,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("41d45d0a-39ea-417f-ba73-888b495525de"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("08becbaf-2a92-4de1-8908-454c4659ad94"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH đạt giải Nhất",
                    ScoreLevel = ScoreLevel.HDSVDatGiaiNhat,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("64780798-ffaf-48eb-be29-8a61fc4854a2"),
                    WorkTypeId = Guid.Parse("e2f7974c-47c3-478e-9b53-74093f6c621f"),
                    WorkLevelId = Guid.Parse("69cc26ee-f6b8-46a6-9229-e42219775d78"),
                    PurposeId = Guid.Parse("bf7e1da9-bb9f-4b64-827c-9b5f114395db"),
                    AuthorRoleId = Guid.Parse("73fa58f9-5877-4c31-92b0-ee5665bc0bee"),
                    Name = "Hướng dẫn đề tài NCKH trường hợp còn lại",
                    ScoreLevel = ScoreLevel.HDSVConLai,
                    MaxAllowed = null,
                    ConvertHour = 20
                },
                // Khác
                new Factor
                {
                    Id = Guid.Parse("7b1f83c1-2d6f-460f-8816-3510673d762a"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapTruong,
                    MaxAllowed = null,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("1fabd534-9220-4fed-91bd-5559b286a20c"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("ee81fe90-15e7-48a2-8d94-a46db55f5b8f"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapTruong,
                    MaxAllowed = null,
                    ConvertHour = 80
                },
                new Factor
                {
                    Id = Guid.Parse("5f1f8da3-5209-485d-82fa-9c09db509e74"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapTinhThanhPho,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("ff6bd8d0-4772-49fc-a1d5-c2659b19c90e"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapTinhThanhPho,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("3f695362-7c44-4f17-a57e-614e68739b94"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapQuocGia,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("7a633c7f-d43f-4cca-926a-1fa35c2d2e17"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapQuocGia,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("96c19b54-7000-40c4-a6de-e8ecff319ae2"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapQuocTe,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("cb5db759-7bf6-42d7-affb-9d8a89382cbc"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Tác phẩm nghệ thuật",
                    ScoreLevel = ScoreLevel.TacPhamNgheThuatCapQuocTe,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("2c6c95c1-cda0-442b-8257-f2b5b94611a8"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Thành tích huấn luyện, thi đấu thể dục thể thao",
                    ScoreLevel = ScoreLevel.ThanhTichHuanLuyenCapQuocGia,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("2971a16e-2ccc-4f36-a6f6-bf28269c2702"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Thành tích huấn luyện, thi đấu thể dục thể thao",
                    ScoreLevel = ScoreLevel.ThanhTichHuanLuyenCapQuocGia,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("7fa77c29-26fb-4b31-b518-b0cb0f37f331"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Thành tích huấn luyện, thi đấu thể dục thể thao",
                    ScoreLevel = ScoreLevel.ThanhTichHuanLuyenCapQuocTe,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("45ad2da3-47b8-4e71-9248-458192dd52c8"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Thành tích huấn luyện, thi đấu thể dục thể thao",
                    ScoreLevel = ScoreLevel.ThanhTichHuanLuyenCapQuocTe,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                new Factor
                {
                    Id = Guid.Parse("767bbcc5-cd12-4789-8677-428169b20d48"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Giải pháp hữu ích",
                    ScoreLevel = ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho,
                    MaxAllowed = null,
                    ConvertHour = 160
                },
                new Factor
                {
                    Id = Guid.Parse("c72c498b-37c6-46cd-b3e1-e82179fd8889"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Giải pháp hữu ích",
                    ScoreLevel = ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho,
                    MaxAllowed = null,
                    ConvertHour = 160
                },

                new Factor
                {
                    Id = Guid.Parse("9ae95c37-54eb-415d-939a-d70008a33b28"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Giải pháp hữu ích",
                    ScoreLevel = ScoreLevel.GiaiPhapHuuIchCapQuocGia,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("568d791f-0a4f-4097-b7e2-9ed9836c3131"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Giải pháp hữu ích",
                    ScoreLevel = ScoreLevel.GiaiPhapHuuIchCapQuocGia,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("99f88266-0281-49b1-bf44-397cf86816d5"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("ee9e27af-859f-4de6-8678-6ae758654931"),
                    Name = "Giải pháp hữu ích",
                    ScoreLevel = ScoreLevel.GiaiPhapHuuIchCapQuocTe,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("12fcd9aa-301b-42f0-9195-d1ca60011613"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = Guid.Parse("5fc7453f-4bc3-4bd2-a8b3-bd99e98a17f5"),
                    Name = "Giải pháp hữu ích",
                    ScoreLevel = ScoreLevel.GiaiPhapHuuIchCapQuocTe,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("e256917b-cb09-4732-bab1-ad10ac407776"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = null,
                    Name = "Kết quả nghiên cứu, ứng dụng khoa học",
                    ScoreLevel = ScoreLevel.KetQuaNghienCuu,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("216ac8a5-228f-47c3-a2c7-451fbba219b7"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("d84ac5f8-d533-48d6-b829-9cf3556ce5bb"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = null,
                    Name = "Kết quả nghiên cứu, ứng dụng khoa học",
                    ScoreLevel = ScoreLevel.KetQuaNghienCuu,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("50d33869-97bc-45a7-a36d-1b031a3c83b5"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("b2302b5e-1614-484d-88ad-003c411ad248"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = null,
                    Name = "Kết quả nghiên cứu, ứng dụng khoa học",
                    ScoreLevel = ScoreLevel.KetQuaNghienCuu,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("8b9c89a7-d95e-45c9-ae7e-9779eed7a225"),
                    WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c"),
                    WorkLevelId = Guid.Parse("13e5b0a5-727b-427b-b103-0d58db679dcd"),
                    PurposeId = Guid.Parse("c27916d9-32b5-4f96-a7f9-7d0a9a0bdfad"),
                    AuthorRoleId = null,
                    Name = "Kết quả nghiên cứu, ứng dụng khoa học",
                    ScoreLevel = ScoreLevel.KetQuaNghienCuu,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Giáo trình - Sách
                new Factor
                {
                    Id = Guid.Parse("e2f96885-2bb7-4668-9c2f-6d6d313c09f7"),
                    WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"),
                    AuthorRoleId = Guid.Parse("be6b03da-7853-48ab-93b3-81da27c3271e"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("9e99441e-be5b-44dc-b0ca-d75bb9797aa1"),
                    WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"),
                    AuthorRoleId = Guid.Parse("8560f2b2-7b9b-4f28-b79a-f5ea21f76e97"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("2971a6c0-9bfa-4c62-91db-e290a9008856"),
                    WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("1e9aa201-0e1b-4214-9dbb-2c9eb59a428a"),
                    AuthorRoleId = Guid.Parse("8bac0cd7-b553-42cc-af1a-5d50d32a6fac"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Chương sách
                new Factor
                {
                    Id = Guid.Parse("d3707663-2b44-4d95-93b7-37756d3e302c"),
                    WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("494e049e-0972-4ff0-a786-6e00880955fc"),
                    AuthorRoleId = Guid.Parse("6304f87f-439a-477d-b989-31df3b6e06b6"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("5eb303f2-4a38-4ecb-a053-6fa2670186fb"),
                    WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("494e049e-0972-4ff0-a786-6e00880955fc"),
                    AuthorRoleId = Guid.Parse("3f0d8b5e-99da-4702-bc34-1b36c99cbdaa"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("cd282c91-bc19-4bac-b0e4-6321cf917221"),
                    WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("494e049e-0972-4ff0-a786-6e00880955fc"),
                    AuthorRoleId = Guid.Parse("98b05ce5-af6e-4953-be9b-45f97e711c86"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Tham khảo
                new Factor
                {
                    Id = Guid.Parse("14b7a7e8-7327-450e-a5ca-f7d836b14499"),
                    WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("3da2c117-b32f-4687-89b8-ba9544920f35"),
                    AuthorRoleId = Guid.Parse("11eea600-4495-486c-985d-57de08b8b5da"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("9eff21e2-bf66-4ff1-a67f-32700c84c412"),
                    WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("3da2c117-b32f-4687-89b8-ba9544920f35"),
                    AuthorRoleId = Guid.Parse("77daab84-939d-4d0d-957d-27be75bb79b4"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("3be58f79-b3c8-4ae2-b761-1d020c064584"),
                    WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("3da2c117-b32f-4687-89b8-ba9544920f35"),
                    AuthorRoleId = Guid.Parse("b0923868-3ce3-4653-97fa-d6925771ce64"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Chuyên khảo
                new Factor
                {
                    Id = Guid.Parse("b1131264-329f-4908-8e71-8b36088d3dde"),
                    WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"),
                    AuthorRoleId = Guid.Parse("5f65dffc-5e3a-46a8-9bc6-1bacce9ef3fa"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("61502bfe-fba5-47d4-9aa9-c1ec01d7fb0f"),
                    WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"),
                    AuthorRoleId = Guid.Parse("4a85d698-7809-4912-923f-18c3f0a2e676"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("53b25eec-2419-4cee-a256-c22767dc3a12"),
                    WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("32cce5b8-24aa-4a3e-9326-c853e5c50fd7"),
                    AuthorRoleId = Guid.Parse("3dfd761c-256e-442f-99fb-136d27b4cea5"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

                // Tài liệu hướng dẫn
                new Factor
                {
                    Id = Guid.Parse("b74daf03-dc04-4738-ae87-97ec0faa07c1"),
                    WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("fc948f99-b569-4265-b1c9-ba5aa31d730b"),
                    AuthorRoleId = Guid.Parse("d8d1af53-3354-4af3-a18f-85c6ee46e750"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("90115bb8-51db-4080-a43e-3383f24a6e8c"),
                    WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("fc948f99-b569-4265-b1c9-ba5aa31d730b"),
                    AuthorRoleId = Guid.Parse("ed76e468-43ee-47c3-8148-e6f63406a98d"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },
                new Factor
                {
                    Id = Guid.Parse("a09df10c-a3cd-45d2-ab09-7f7a7a36aeed"),
                    WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0"),
                    WorkLevelId = null,
                    PurposeId = Guid.Parse("fc948f99-b569-4265-b1c9-ba5aa31d730b"),
                    AuthorRoleId = Guid.Parse("3016bf69-e8d1-4852-a717-b5924a7bb7b2"),
                    Name = "Sách",
                    ScoreLevel = ScoreLevel.Sach,
                    MaxAllowed = null,
                    ConvertHour = 240
                },

            };

            builder.HasData(factors);
        }
    }

    public class SCImagoFieldSeeding : IEntityTypeConfiguration<SCImagoField>
    {
        public void Configure(EntityTypeBuilder<SCImagoField> builder)
        {
            var scimagoFields = new List<SCImagoField>
                {
                    new SCImagoField { Id = Guid.Parse("71d16d65-5823-41b6-975d-b8189c41481b"), Name = "Accounting" },
                    new SCImagoField { Id = Guid.Parse("401fcfa1-b021-47a9-876e-4c2af8ebb470"), Name = "Analysis" },
                    new SCImagoField { Id = Guid.Parse("5d650d2a-dcbf-4efc-bb3d-9cdbce0c207e"), Name = "Anatomy" }
                };

            builder.HasData(scimagoFields);
        }

    }
    public class AcademicYearSeeding : IEntityTypeConfiguration<AcademicYear>
    {
        public void Configure(EntityTypeBuilder<AcademicYear> builder)
        {
            var academicYears = new List<AcademicYear>
                {
                    new AcademicYear { Id = Guid.Parse("e53bc8e5-a17e-4a9b-a403-0e1b7d3118a2"), Name = "2023-2024", StartDate = new DateOnly(2023, 9, 1), EndDate = new DateOnly(2024, 6, 30) },
                    new AcademicYear { Id = Guid.Parse("dab343ac-b1a8-45b4-a7f8-a4260594d7d8"), Name = "2024-2025", StartDate = new DateOnly(2024, 9, 1), EndDate = new DateOnly(2025, 6, 30) },
                    new AcademicYear { Id = Guid.Parse("33fdb5af-0778-4d91-8b68-dce2860e138c"), Name = "2025-2026", StartDate = new DateOnly(2025, 9, 1), EndDate = new DateOnly(2026, 6, 30) },

                };
            builder.HasData(academicYears);
        }
    }
}