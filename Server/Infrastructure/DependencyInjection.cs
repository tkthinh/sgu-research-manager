using Application.Departments;
using Application.Fields;
using Application.Purposes;
using Application.WorkLevels;
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
using Infrastructure.Identity;
using Application.WorkTypes;
using Application.AuthorRoles;
using Application.Assignments;
using Application.Factors;
using Application.Authors;
using Application.Works;
using Application.SCImagoFields;
using Infrastructure.Data.Repositories;
using Application.Users;
using Application.SystemConfigs;
using Application.Caches;
using StackExchange.Redis;
using Application.ScoreLevels;
using Infrastructure.Identity.Services;
using Application.Auth;
using Application.AcademicYears;
using Application.AuthorRegistrations;
using Application.Shared.Services;
using Application.Notifications;

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

            // Cấu hình Identity với SQL Server
            services.AddDbContext<AuthDbContext>(options =>
               options.UseSqlServer(
                  configuration.GetConnectionString("DefaultConnection"),
                  b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            });

            // Cấu hình Redis Cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            // Đăng ký Identity
            services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
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

            // Đăng ký HttpContextAccessor
            services.AddHttpContextAccessor();

            // Đăng ký SignalR
            services.AddSignalR();

            // Đăng ký các service
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IPurposeService, PurposeService>();
            services.AddScoped<IWorkLevelService, WorkLevelService>();
            services.AddScoped<IFieldService, FieldService>();
            services.AddScoped<IWorkTypeService, WorkTypeService>();
            services.AddScoped<IAuthorRoleService, AuthorRoleService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IFactorService, FactorService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ISCImagoFieldService, SCImagoFieldService>();
            services.AddScoped<IScoreLevelService, ScoreLevelService>();
            services.AddScoped<IAcademicYearService, AcademicYearService>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();
            services.AddScoped<IAuthorRegistrationService, AuthorRegistrationService>();
            services.AddScoped<INotificationService, NotificationService>();

            services.AddScoped<ICacheManagementService, CacheManagementService>();
            services.AddSingleton(ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserImportService, UserImportService>();

            // Đăng ký các mapper
            services.AddScoped<IGenericMapper<DepartmentDto, Department>, DepartmentMapper>();
            services.AddScoped<IGenericMapper<PurposeDto, Purpose>, PurposeMapper>();
            services.AddScoped<IGenericMapper<WorkLevelDto, WorkLevel>, WorkLevelMapper>();
            services.AddScoped<IGenericMapper<FieldDto, Field>, FieldMapper>();
            services.AddScoped<IGenericMapper<WorkTypeDto, WorkType>, WorkTypeMapper>();
            services.AddScoped<IGenericMapper<AuthorRoleDto, AuthorRole>, AuthorRoleMapper>();
            services.AddScoped<IGenericMapper<AssignmentDto, Assignment>, AssignmentMapper>();
            services.AddScoped<IGenericMapper<FactorDto, Factor>, FactorMapper>();
            services.AddScoped<IGenericMapper<AuthorDto, Author>, AuthorMapper>();
            services.AddScoped<IGenericMapper<WorkDto, Work>, WorkMapper>();
            services.AddScoped<IGenericMapper<UserDto, User>, UserMapper>();
            services.AddScoped<IGenericMapper<SCImagoFieldDto, SCImagoField>, SCImagoFieldMapper>();
            services.AddScoped<IGenericMapper<SystemConfigDto, SystemConfig>, SystemConfigMapper>();
            services.AddScoped<IGenericMapper<AcademicYearDto, AcademicYear>, AcademicYearMapper>();
            services.AddScoped<IGenericMapper<AuthorRegistrationDto, AuthorRegistration>, AuthorRegistrationMapper>();
            services.AddScoped<IGenericMapper<NotificationDto, Notification>, NotificationMapper>();


            // Đăng ký custom repository (nếu có)
            services.AddScoped<IWorkTypeRepository, WorkTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWorkRepository, WorkRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            return services;
        }
    }
}
