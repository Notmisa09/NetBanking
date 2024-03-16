using Microsoft.AspNetCore.Identity;

namespace NetBanking.Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public bool UserStatus { get; set; }
    }
}
