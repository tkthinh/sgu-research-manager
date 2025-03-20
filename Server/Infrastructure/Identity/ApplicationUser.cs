using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
   public class ApplicationUser : IdentityUser
   {
      public bool IsApproved { get; set; } = false;
   }
}
