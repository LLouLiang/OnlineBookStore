using Microsoft.AspNetCore.Identity;

namespace OnlineBookStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
