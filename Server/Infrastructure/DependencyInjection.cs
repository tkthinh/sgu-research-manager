using Application.Departments;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

         // dang ky uow
         services.AddScoped<IUnitOfWork, UnitOfWork>();

         //dang ky services
         services.AddScoped<IDepartmentService, DepartmentService>();

         //dang ky mapper
         services.AddScoped<IGenericMapper<DepartmentDto, Department>, DepartmentMapper>();


         return services;
      }
   }
}
