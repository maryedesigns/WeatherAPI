using Microsoft.AspNetCore.Identity;

namespace WeatherProject.Models
{
    public class User : IdentityUser
    {
        //public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }

    }
}
