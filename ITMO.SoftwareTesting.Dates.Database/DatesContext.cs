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
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserAtGroup> UsersAtGroups { get; set; }
        public DbSet<GroupInvitation> GroupInvitations { get; set; }
        public DbSet<FavoriteUser> FavoriteUsers { get; set; }
        public DbSet<FavoriteEvent> FavoriteEvents { get; set; }

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