using Application.AcademicRanks;
using Application.Departments;
using Application.Fields;
using Application.OfficerRanks;
using Application.Purposes;
using Application.WorkLevels;
using Application.WorkStatuses;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Cấu hình Entity Framework Core với SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));


         services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

            // Cấu hình Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

         // dang ky identity
         services.AddIdentityCore<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("")
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

         services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                  AuthenticationType = "Jwt",
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = configuration["Jwt:Issuer"],
                  ValidAudience = configuration["Jwt:Audience"],
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
               };
            });

         // Đăng ký UnitOfWork
         services.AddScoped<IUnitOfWork, UnitOfWork>();

         
         // Đăng ký các service
         services.AddScoped<IDepartmentService, DepartmentService>();
         services.AddScoped<IPurposeService, PurposeService>();
         services.AddScoped<IWorkLevelService, WorkLevelService>();
         services.AddScoped<IFieldService, FieldService>();
         services.AddScoped<IAcademicRankService, AcademicRankService>();
         services.AddScoped<IOfficerRankService, OfficerRankService>();
         services.AddScoped<IWorkStatusService, WorkStatusService>();


         // Đăng ký các mapper
         services.AddScoped<IGenericMapper<DepartmentDto, Department>, DepartmentMapper>();
         services.AddScoped<IGenericMapper<PurposeDto, Purpose>, PurposeMapper>();
         services.AddScoped<IGenericMapper<WorkLevelDto, WorkLevel>, WorkLevelMapper>();
         services.AddScoped<IGenericMapper<FieldDto, Field>, FieldMapper>();
         services.AddScoped<IGenericMapper<AcademicRankDto, AcademicRank>, AcademicRankMapper>();
         services.AddScoped<IGenericMapper<OfficerRankDto, OfficerRank>, OfficerRankMapper>();
         services.AddScoped<IGenericMapper<WorkStatusDto, WorkStatus>, WorkStatusMapper>();

         return services;
        }
    }
}
