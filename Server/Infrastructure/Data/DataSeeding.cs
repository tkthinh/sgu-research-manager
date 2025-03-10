using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
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
                    new WorkLevel {
                        Id = Guid.Parse("f63f1ff3-f33b-4c19-aa00-6f2206e65b07"),
                        Name = "Khoa",
                        WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348")
                    },
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
                        IsMainAuthor = false,
                        WorkTypeId = Guid.Parse("140a3e34-ded1-4bfa-8633-fbea545cbdaa")
                    },
                    new AuthorRole {
                        Id = Guid.Parse("4be849d3-b55d-429a-a0b3-78c4bbbcd7eb"),
                        Name = "Ban Biên tập kỹ yếu",
                        IsMainAuthor = false,
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
                // Báo cáo khoa học - Không có ScoreLevel (Score = 0 trước đây -> null)
                new Factor
                {
                    Id = Guid.Parse("ad619718-0937-4693-a40f-04e56241a52c"),
                    Name = "Báo cáo khoa học - Trường - Quy đổi giờ nghĩa vụ",
                    ScoreLevel = null, // Không có mức điểm
                    ConvertHour = 40,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("3c21b247-16ce-40a9-a921-abef0e1bba56"),
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                    AuthorRoleId = null // Nếu không có AuthorRoleId, để null
                },
                new Factor
                {
                    Id = Guid.Parse("60f6bc8a-05ff-4322-bccd-5ea9335139c1"),
                    Name = "Báo cáo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ",
                    ScoreLevel = null,
                    ConvertHour = 80,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("2ba0b318-cb6f-49a0-b6a3-9040dcc46a9b"),
                    Name = "Báo cáo khoa học - Quốc gia - Quy đổi giờ nghĩa vụ",
                    ScoreLevel = null,
                    ConvertHour = 80,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("146e3c5e-7bee-41a6-9f1b-8f00ee2a4eb7"),
                    Name = "Báo cáo khoa học - Bộ/Ngành - Quy đổi vượt định mức",
                    ScoreLevel = null,
                    ConvertHour = 54,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("250662c1-1c69-4ef0-a21d-7077cafd1d06"),
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("3c39ab6a-8c61-4321-b498-4a3a469ea1cc"),
                    Name = "Báo cáo khoa học - Quốc gia - Quy đổi vượt định mức",
                    ScoreLevel = null,
                    ConvertHour = 54,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("740e8212-f47b-4080-b57a-839b8b90056c"),
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("30775a5a-9282-4801-80f2-acaea95c5f71"),
                    Name = "Báo cáo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ",
                    ScoreLevel = null,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("4f163c6f-ad7b-4f38-8125-0584678164b6"),
                    Name = "Báo cáo khoa học - Quốc tế - Quy đổi vượt định mức",
                    ScoreLevel = null,
                    ConvertHour = 76,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("db324190-d1ed-4712-b3db-94a6e043bf1e"),
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("cbd4799e-ec4b-41a7-8eb2-744841178857"),
                    Name = "Báo cáo khoa học - WoS - Quy đổi giờ nghĩa vụ",
                    ScoreLevel = null,
                    ConvertHour = 200,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("415d12ed-7551-48fd-8a5c-87e02fee0dd3"),
                    Name = "Báo cáo khoa học - WoS - Quy đổi vượt định mức",
                    ScoreLevel = null,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("f81c134b-fd83-4e25-9590-cf7ecfc5b203"),
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("86f1114c-73b7-4c4f-bac7-95b602bcc397"),
                    Name = "Báo cáo khoa học - Scopus - Quy đổi giờ nghĩa vụ",
                    ScoreLevel = null,
                    ConvertHour = 200,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),
                    PurposeId = Guid.Parse("340bd6e7-9d49-4650-a4cf-f1928358aa7c"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("31673044-6195-4d65-a1f0-453cee604604"),
                    Name = "Báo cáo khoa học - Scopus - Quy đổi vượt định mức",
                    ScoreLevel = null,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4"),
                    WorkLevelId = Guid.Parse("f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9"),
                    PurposeId = Guid.Parse("db5d595d-e4be-4640-ab4e-ca4269d9b1cd"),
                    AuthorRoleId = null
                },

                // Bài báo khoa học - WoS và Scopus
                new Factor
                {
                    Id = Guid.Parse("c5c436fe-fac7-4243-afd7-a07ba9fa6113"),
                    Name = "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 10",
                    ScoreLevel = ScoreLevel.TenPercent, // 10%
                    ConvertHour = 600,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("c7fc7ef4-dc3e-473f-af05-5514f6c223e8"),
                    Name = "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 10",
                    ScoreLevel = ScoreLevel.TenPercent,
                    ConvertHour = 800,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("849b4d7e-3928-45b6-8f4d-17c078bbcc7f"),
                    Name = "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 30",
                    ScoreLevel = ScoreLevel.ThirtyPercent, // 30%
                    ConvertHour = 400,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("5e382f01-49b7-4910-bf99-cdcddd5042e3"),
                    Name = "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 30",
                    ScoreLevel = ScoreLevel.ThirtyPercent,
                    ConvertHour = 640,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("43454113-745b-4eee-b266-ff515ed9027b"),
                    Name = "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 50",
                    ScoreLevel = ScoreLevel.FiftyPercent, // 50%
                    ConvertHour = 240,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("aeb8a311-304e-4c6e-b944-7cdafac6947b"),
                    Name = "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 50",
                    ScoreLevel = ScoreLevel.FiftyPercent,
                    ConvertHour = 560,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("b0a80a7a-9703-4af2-b154-bb4a5bd9c315"),
                    Name = "Bài báo khoa học - WoS - Quy đổi vượt định mức - Score 100",
                    ScoreLevel = ScoreLevel.HundredPercent, // 100%
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("35ba841d-42b2-425a-9e27-82b52a81dc73"),
                    Name = "Bài báo khoa học - WoS - Quy đổi giờ nghĩa vụ - Score 100",
                    ScoreLevel = ScoreLevel.HundredPercent,
                    ConvertHour = 400,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("0b031a2d-4ac5-48fb-9759-f7a2fe2f7290"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("c2288e08-eb6b-4cd0-91ce-9adf4ee8e745"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 10",
                    ScoreLevel = ScoreLevel.TenPercent,
                    ConvertHour = 600,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("10c586ce-79ec-4859-881f-5d38245ce47b"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 10",
                    ScoreLevel = ScoreLevel.TenPercent,
                    ConvertHour = 800,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("2fca54b4-555a-4d67-9e9a-f522e0c802cb"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 30",
                    ScoreLevel = ScoreLevel.ThirtyPercent,
                    ConvertHour = 400,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("0fa0aa31-978d-42be-8adc-a47729ff9b9d"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 30",
                    ScoreLevel = ScoreLevel.ThirtyPercent,
                    ConvertHour = 640,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("85f22829-055a-4314-a8dc-649a14346610"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 50",
                    ScoreLevel = ScoreLevel.FiftyPercent,
                    ConvertHour = 240,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("38f2bbe7-8e32-477a-85bf-e2a08fb88c03"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 50",
                    ScoreLevel = ScoreLevel.FiftyPercent,
                    ConvertHour = 560,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("9c4ed0a6-b71a-46ea-a27f-02b38bd0c544"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi vượt định mức - Score 100",
                    ScoreLevel = ScoreLevel.HundredPercent,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("e4f74734-e0f5-446d-8221-cc3a519ad461"),
                    Name = "Bài báo khoa học - Scopus - Quy đổi giờ nghĩa vụ - Score 100",
                    ScoreLevel = ScoreLevel.HundredPercent,
                    ConvertHour = 400,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("34f94668-7151-457d-aa06-4bf4e2b27df3"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },

                // Bài báo khoa học - Trường, Bộ/Ngành, Quốc tế (Score = 1)
                new Factor
                {
                    Id = Guid.Parse("92f2fdef-e1cf-4062-aa96-f01c1820bb98"),
                    Name = "Bài báo khoa học - Trường - Quy đổi vượt định mức - Score 1",
                    ScoreLevel = ScoreLevel.One, // 1
                    ConvertHour = 76,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("779bdc74-0043-46a0-84ad-940af4f4dc49"),
                    Name = "Bài báo khoa học - Trường - Quy đổi giờ nghĩa vụ - Score 1",
                    ScoreLevel = ScoreLevel.One,
                    ConvertHour = 160,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("decaf6cd-fb66-440c-9cc3-155036dfc775"),
                    Name = "Bài báo khoa học - Bộ/Ngành - Quy đổi vượt định mức - Score 1",
                    ScoreLevel = ScoreLevel.One,
                    ConvertHour = 76,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("f56495e4-eac2-47f5-b282-d3f39055fecb"),
                    Name = "Bài báo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ - Score 1",
                    ScoreLevel = ScoreLevel.One,
                    ConvertHour = 160,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("8695bc4f-992a-4b44-ad3e-a373f88672f4"),
                    Name = "Bài báo khoa học - Quốc tế - Quy đổi vượt định mức - Score 1",
                    ScoreLevel = ScoreLevel.One,
                    ConvertHour = 76,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("630d6ebe-34a2-4533-9a06-ec28ad6d1cd4"),
                    Name = "Bài báo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ - Score 1",
                    ScoreLevel = ScoreLevel.One,
                    ConvertHour = 160,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },

                // Bài báo khoa học - Trường, Bộ/Ngành, Quốc tế (Score = 0.75)
                new Factor
                {
                    Id = Guid.Parse("5a01f52c-999f-447e-bb2d-2fb4c9161d25"),
                    Name = "Bài báo khoa học - Trường - Quy đổi vượt định mức - Score 0.75",
                    ScoreLevel = ScoreLevel.ZeroPointSevenFive, // 0.75
                    ConvertHour = 54,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("4f47632f-9796-45fa-b4a5-e0c39be496e9"),
                    Name = "Bài báo khoa học - Trường - Quy đổi giờ nghĩa vụ - Score 0.75",
                    ScoreLevel = ScoreLevel.ZeroPointSevenFive,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("d473791e-fb18-409a-be03-a0b60c75912c"),
                    Name = "Bài báo khoa học - Bộ/Ngành - Quy đổi vượt định mức - Score 0.75",
                    ScoreLevel = ScoreLevel.ZeroPointSevenFive,
                    ConvertHour = 54,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("45892e0d-6abb-41ea-8e29-c34c05b58068"),
                    Name = "Bài báo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ - Score 0.75",
                    ScoreLevel = ScoreLevel.ZeroPointSevenFive,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("54740838-3d63-43b4-a498-9c5152dd7528"),
                    Name = "Bài báo khoa học - Quốc tế - Quy đổi vượt định mức - Score 0.75",
                    ScoreLevel = ScoreLevel.ZeroPointSevenFive,
                    ConvertHour = 54,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("c8785d2b-244a-40ac-9430-7c416adefbc9"),
                    Name = "Bài báo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ - Score 0.75",
                    ScoreLevel = ScoreLevel.ZeroPointSevenFive,
                    ConvertHour = 120,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },

                // Bài báo khoa học - Trường, Bộ/Ngành, Quốc tế (Score = 0.5)
                new Factor
                {
                    Id = Guid.Parse("5386b122-9311-453e-ab70-de37d9673ef5"),
                    Name = "Bài báo khoa học - Trường - Quy đổi vượt định mức - Score 0.5",
                    ScoreLevel = ScoreLevel.ZeroPointFive, // 0.5
                    ConvertHour = 34,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("bd63c850-3c87-4836-9a7e-7c402ad436cf"),
                    Name = "Bài báo khoa học - Trường - Quy đổi giờ nghĩa vụ - Score 0.5",
                    ScoreLevel = ScoreLevel.ZeroPointFive,
                    ConvertHour = 80,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("23dad081-62db-4944-87d2-43b29c31fa29"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("3236b11f-a2ac-4e51-85d1-fcc5594455b6"),
                    Name = "Bài báo khoa học - Bộ/Ngành - Quy đổi vượt định mức - Score 0.5",
                    ScoreLevel = ScoreLevel.ZeroPointFive,
                    ConvertHour = 34,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("fcd3b42b-7151-4100-8f08-0019c14a764c"),
                    Name = "Bài báo khoa học - Bộ/Ngành - Quy đổi giờ nghĩa vụ - Score 0.5",
                    ScoreLevel = ScoreLevel.ZeroPointFive,
                    ConvertHour = 80,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("2d8e237a-bdb3-4d8c-b20a-860f23f65627"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("ce72ef2c-a629-40ea-9bab-d77b87421fdf"),
                    Name = "Bài báo khoa học - Quốc tế - Quy đổi vượt định mức - Score 0.5",
                    ScoreLevel = ScoreLevel.ZeroPointFive,
                    ConvertHour = 34,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                    PurposeId = Guid.Parse("e6fdbc77-108d-443a-85c4-3c8c361f7f3b"),
                    AuthorRoleId = null
                },
                new Factor
                {
                    Id = Guid.Parse("b7c82548-b2cd-4368-8b6e-919f7f6b1e5f"),
                    Name = "Bài báo khoa học - Quốc tế - Quy đổi giờ nghĩa vụ - Score 0.5",
                    ScoreLevel = ScoreLevel.ZeroPointFive,
                    ConvertHour = 80,
                    WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a"),
                    WorkLevelId = Guid.Parse("b1f4b511-99fc-49a5-a82a-99e1ebb2207d"),
                    PurposeId = Guid.Parse("34fe4df6-0a28-4ddf-930f-19e5febebdee"),
                    AuthorRoleId = null
                }
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
                    // Accounting
                    new SCImagoField { Id = Guid.Parse("71d16d65-5823-41b6-975d-b8189c41481b"), Name = "Accounting", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("401fcfa1-b021-47a9-876e-4c2af8ebb470"), Name = "Accounting", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("5d650d2a-dcbf-4efc-bb3d-9cdbce0c207e"), Name = "Accounting", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("9e2e9363-7202-4fec-afe7-ebef07a882e6"), Name = "Accounting", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("4ea5768f-e908-41d1-875a-fafe00d072d6"), Name = "Accounting", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("b6bf371a-d91f-43d1-8920-12db377ff70f"), Name = "Accounting", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("21dc2bc6-9a66-4126-8df4-550bf46e834b"), Name = "Accounting", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("968bfb7c-3371-4a66-9fa0-7e6bba2e6bc7"), Name = "Accounting", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("05552af0-6e6d-40cd-8dcd-90204f20bfca"), Name = "Accounting", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("b9fd2cc9-cdd8-405c-b616-20e16dcd8fc0"), Name = "Accounting", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Acoustics and Ultrasonics
                    new SCImagoField { Id = Guid.Parse("496579af-c43f-4aee-987e-f7bd5ee7fc4e"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("e249a273-360b-40bb-bb52-3b2e066bf648"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("3b550a89-4f41-4338-9b59-86e125d799e8"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("9ca69376-adbd-4e06-959e-364e739d5e1d"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("70396506-ba86-410f-b09e-7db24e1f7b19"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("92433e34-b419-48e0-ac63-45bc4303c5b3"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("41e1107a-6f87-493b-ac8e-13479ef48fb9"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("73bd013c-6d0a-4b32-aeac-b1df414ad8be"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("6dbe6812-1775-429e-97f4-e39526e8d95d"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("0bfe40c5-db06-41a9-8a20-8fefc7b8bc56"), Name = "Acoustics and Ultrasonics", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Advanced and Specialized Nursing
                    new SCImagoField { Id = Guid.Parse("26ed0b8a-e453-4882-b2c7-9d8b18baca4e"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("5e4f7453-8d53-4af7-a14c-d4539abbc2b4"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("b91e4e0b-a3e1-4694-ac6e-041a259f98e9"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("cc970669-ecbb-43e9-829c-6ef73382b868"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("8b1416d0-6265-4284-b6ec-db01db76e59e"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("9d81951c-19b7-425d-9e35-c39a9251b1c1"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("2d518f0c-4611-426e-883d-4192bda56371"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("37d326bf-2bdf-44b9-9fac-043066058006"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("2122ea3c-d666-45d9-933a-57e0c853d77a"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("1aad5fb3-4742-43a6-b004-d38cea7554e5"), Name = "Advanced and Specialized Nursing", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Aerospace Engineering
                    new SCImagoField { Id = Guid.Parse("72b465d7-e634-4878-a3dc-d42165da4f20"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("cf9737f8-e9af-42ff-9a6a-9791602676ad"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("4e32a1e4-1360-438b-87d8-2f4f273dd01e"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("a803230c-efa0-49d9-99d6-b8d3c7c9bc48"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("b683926d-a267-4ad3-b387-ce17eab9acb9"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("6b652d84-6e79-4a60-b8ee-8b07f1da0fae"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("2566aaaf-185b-424a-ac0a-4373f08be1cd"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("3a6733ce-1858-4628-b34d-0b96ebe3a6c1"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("31f4e8a8-e50c-46a8-8a69-07d15fea8374"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("ad5f1ec7-f451-4122-918f-0d389e4293a3"), Name = "Aerospace Engineering", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Aging
                    new SCImagoField { Id = Guid.Parse("a94f435c-ba53-4350-890d-77d7a38ab197"), Name = "Aging", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("59361118-b41c-4718-88c1-16aac146337a"), Name = "Aging", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("73c183be-4203-4c11-be2b-1eb327f61a4b"), Name = "Aging", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("a4c01d6d-bc55-4f8a-88f3-f0caa52019e1"), Name = "Aging", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("173a0f0e-9516-488c-a5dd-531478e7842f"), Name = "Aging", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("401a8622-0680-495d-8da6-47e739effd62"), Name = "Aging", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("83409cd3-b669-413c-a71c-7a1a0ff761c5"), Name = "Aging", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("86237475-2dd4-4c6a-b37a-f5d9dcd235a4"), Name = "Aging", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("ad36a77b-bbbd-497e-8929-8a6703a3e397"), Name = "Aging", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("39f200aa-04e3-4d19-b60d-f0167e5901af"), Name = "Aging", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Agricultural and Biological Sciences (miscellaneous)
                    new SCImagoField { Id = Guid.Parse("f97d98e9-5eb6-4b82-9613-ad2e88988a3a"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("53c9682c-54c6-4515-9414-7ed2a5ab9dbd"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("7aa65d95-c4e7-457d-aa3b-e9df7684fe4c"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("80435537-dbfd-49b0-8952-d9e6c67289b4"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("b1340f55-49a9-45d7-bd81-7c6e6c156dbc"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("e0da499d-f636-463f-b407-c503754687d9"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("f54ed2ce-4385-486f-a96f-203aff849298"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("a25484e5-bcb4-47f7-8a86-a4f8cd488b3a"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("e73c1b54-61b8-4ecf-a831-523b12926c17"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("f7d2ace6-bad7-49d5-af0b-73b49678483e"), Name = "Agricultural and Biological Sciences (miscellaneous)", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Agronomy and Crop Science
                    new SCImagoField { Id = Guid.Parse("b0de64d1-ce89-4800-b24a-c2d2ef327ef5"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("30120b72-4c68-46da-9cfd-c275c87c5b4b"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("71b2b969-44a1-4953-994f-693c851e0bf6"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("7beec1fe-625c-4461-804b-7dc40e6e34dc"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("d1aa9f38-b6a0-46f4-a099-a1f5a2c3612a"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("514a1695-a534-4c68-85f3-b7d7f3c2cf6a"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("566ebe27-f2e3-431c-91b3-36864f9531cb"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("30ef4e0a-f1c4-4e02-b81f-96debeea8ba7"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("acc93d1f-a550-4f69-9cf4-eb277421e0c3"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("b81a7ded-6768-4d94-aad1-e9f60e8d9d60"), Name = "Agronomy and Crop Science", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Algebra and Number Theory
                    new SCImagoField { Id = Guid.Parse("4cdc5166-08dc-4eb7-acb0-9e0c2c9547b7"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("d8966118-e417-485d-870a-ba0c35581413"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("6d266310-a83e-4367-b1ef-a331e475db7e"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("17fa40fc-295f-47b8-a573-090b280fb201"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("7edc3d54-6f58-4b39-8252-1d02fe836d18"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("b4aa636d-c892-446a-a2d0-bace85fa681c"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("e607986e-e8b7-44d6-8ef9-ed7bf3796e97"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("ed1fae08-4468-49c2-85f0-ce964ab80d2b"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("76842dc6-38a9-4ec6-9ac8-20f298eb09a1"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("3657d8a5-9ca3-4310-af30-8c919f1d0ddc"), Name = "Algebra and Number Theory", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Analysis
                    new SCImagoField { Id = Guid.Parse("cca5e727-1820-4f59-a929-ab7932e97830"), Name = "Analysis", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("429f30db-d831-4909-92d9-8642ac476c5d"), Name = "Analysis", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("8c1d9602-dff1-4592-8ffc-3abf18f83707"), Name = "Analysis", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("08852f72-cfc1-4e62-ba27-5d33dc5b894f"), Name = "Analysis", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("0546a881-8ac0-4ff3-9145-672ad8ee1384"), Name = "Analysis", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("69b6c01d-af65-4bc7-afc3-a0a917fc0e4c"), Name = "Analysis", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("81fbf356-d99b-4911-b596-3b723910c5de"), Name = "Analysis", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("736edc0a-1918-486a-8508-be2a3729bbe6"), Name = "Analysis", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("96c82acf-938d-41af-bb1b-716e8136a925"), Name = "Analysis", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("38cabc00-a4ec-4a5e-abd3-51394cdcdb1d"), Name = "Analysis", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác

                    // Analytical Chemistry
                    new SCImagoField { Id = Guid.Parse("d828e4c5-6fec-43fd-914a-d7860f349874"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("2732c858-77dc-471d-bd9a-464a3142530a") }, // Bài báo khoa học
                    new SCImagoField { Id = Guid.Parse("bd40abcc-83cb-4b04-b907-9f5d14aaa736"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("03412ca7-8ccf-4903-9018-457768060ab4") }, // Báo cáo khoa học
                    new SCImagoField { Id = Guid.Parse("d825ee2a-83e0-4692-a7ca-8987be1926a2"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("49cf7589-fb84-4934-be8e-991c6319a348") }, // Đề tài
                    new SCImagoField { Id = Guid.Parse("397ff495-3f2c-448a-95ca-0ef9eaecd493"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("323371c0-26c7-4549-90f2-11c881be402d") }, // Giáo trình
                    new SCImagoField { Id = Guid.Parse("59344f92-394a-49a7-afbc-673731e2beed"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("3bbfc66a-3144-4edf-959b-e049d7e33d97") }, // Chương sách
                    new SCImagoField { Id = Guid.Parse("05ccc5da-0496-46c8-ada2-5d6a0e466536"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("61bbbecc-038a-43b7-aafa-a95e25a93f38") }, // Chuyên khảo
                    new SCImagoField { Id = Guid.Parse("9520ec93-3438-4576-804e-0a3b0dd5d9ab"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("628a119e-324f-42b8-8ff4-e29ee5c643a9") }, // Tham khảo
                    new SCImagoField { Id = Guid.Parse("3cfc940b-753d-4286-b2cc-274108045404"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("84a14a8b-eae8-4720-bc7c-e1f93b35a256") }, // Giáo trình - Sách
                    new SCImagoField { Id = Guid.Parse("2df7bd79-06fe-4c34-89c5-0c8c9aa99300"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0") }, // Tài liệu hướng dẫn
                    new SCImagoField { Id = Guid.Parse("f2005a34-e48e-47b1-a4af-53396d4bc96c"), Name = "Analytical Chemistry", WorkTypeId = Guid.Parse("1ff8d087-e0c3-45df-befc-662c0a80c10c") }, // Khác
                };

            builder.HasData(scimagoFields);
        }
    }
}