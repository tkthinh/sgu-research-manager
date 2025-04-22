using System.Text;
using Application.AcademicYears;
using Application.Assignments;
using Application.Auth;
using Application.AuthorRegistrations;
using Application.AuthorRoles;
using Application.Authors;
using Application.Caches;
using Application.Departments;
using Application.Factors;
using Application.Fields;
using Application.Notifications;
using Application.Purposes;
using Application.SCImagoFields;
using Application.ScoreLevels;
using Application.Shared.Services;
using Application.SystemConfigs;
using Application.Users;
using Application.WorkLevels;
using Application.Works;
using Application.WorkTypes;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.UnitOfWork;
using Infrastructure.Identity;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

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
            var redisOpts = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis")!);
            redisOpts.AbortOnConnectFail = false;
            redisOpts.ConnectRetry = 3;
            redisOpts.ConnectTimeout = 5000;

            services.AddStackExchangeRedisCache(opts =>
            {
                opts.ConfigurationOptions = redisOpts;
            });

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                try
                {
                    return ConnectionMultiplexer.Connect(redisOpts);
                }
                catch (Exception ex)
                {
                    var log = sp.GetRequiredService<ILogger>();
                    log.LogWarning(ex, "RedisMultiplexer failed to connect—cache disabled for now");
                    return null!;
                }
            });

            // Đăng ký Identity
            services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                       NameClaimType = "id"
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           // IMPORTANT: update the path check based on your hub route!
                           var accessToken = context.Request.Query["access_token"];
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notification-hub"))
                           {
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };
               });

            // Đăng ký UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Đăng ký HttpContextAccessor
            services.AddHttpContextAccessor();

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
            services.AddScoped<IWorkQueryService, WorkQueryService>();
            services.AddScoped<IWorkExportService, WorkExportService>();
            services.AddScoped<IWorkImportService, WorkImportService>();
            services.AddScoped<IWorkCalculateService, WorkCalculateService>();
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
