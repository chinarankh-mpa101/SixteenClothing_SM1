using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SixteenClothing.Models
{
    public class AppUser:IdentityUser
    {
        public string Fullname { get; set; }
    }
}
