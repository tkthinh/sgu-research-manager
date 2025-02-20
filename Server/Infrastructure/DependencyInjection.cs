using Application.Departments;
using Application.Purposes;
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
         services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

         services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(
               configuration.GetConnectionString("DefaultConnection"),
               b => b.MigrationsAssembly(typeof(AuthDbContext).Assembly.FullName)));

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

         // dang ky uow
         services.AddScoped<IUnitOfWork, UnitOfWork>();

         //dang ky services
         services.AddScoped<IDepartmentService, DepartmentService>();
         services.AddScoped<IPurposeService, PurposeService>();

         //dang ky mapper
         services.AddScoped<IGenericMapper<DepartmentDto, Department>, DepartmentMapper>();
         services.AddScoped<IGenericMapper<PurposeDto, Purpose>, PurposeMapper>();


         return services;
      }
   }
}
