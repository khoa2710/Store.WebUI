using Microsoft.AspNetCore.Identity;

namespace Store.WebUI.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? AvataUrl { get; set; }

    }
}
