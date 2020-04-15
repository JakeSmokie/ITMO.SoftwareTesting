using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Database
{
    public class DatesContext : DbContext
    {
        public DatesContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}