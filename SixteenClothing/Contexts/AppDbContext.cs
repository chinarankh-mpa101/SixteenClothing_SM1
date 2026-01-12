using Microsoft.EntityFrameworkCore;
using SixteenClothing.Models;
using System.Reflection;

namespace SixteenClothing.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet <Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
