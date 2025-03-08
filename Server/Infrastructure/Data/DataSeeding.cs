using Domain.Entities;
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

   public class IdentityUserSeeding : IEntityTypeConfiguration<IdentityUser>
   {
      public void Configure(EntityTypeBuilder<IdentityUser> builder)
      {
         var hasher = new PasswordHasher<IdentityUser>();
         var users = new List<IdentityUser>
                {
                    new IdentityUser
                    {
                        UserName = "admin",
                        NormalizedUserName = "ADMIN",
                        Email = "admin@localhost",
                        NormalizedEmail = "ADMIN@LOCALHOST",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "Admin@123"),
                        SecurityStamp = string.Empty
                    }
                };
         builder.HasData(users);
      }
   }
}