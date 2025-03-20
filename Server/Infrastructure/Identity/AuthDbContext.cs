using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
   public class AuthDbContext : IdentityDbContext<ApplicationUser>
   {
      public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
      {

      }
   }
}
