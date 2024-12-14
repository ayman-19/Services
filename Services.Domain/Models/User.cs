using Microsoft.AspNetCore.Identity;

namespace Services.Domain.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
    }
}
