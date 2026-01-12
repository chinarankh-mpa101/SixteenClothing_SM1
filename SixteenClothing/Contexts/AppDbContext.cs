using Microsoft.EntityFrameworkCore;

namespace SixteenClothing.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}
