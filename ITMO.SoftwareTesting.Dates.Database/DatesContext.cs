using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ITMO.SoftwareTesting.Dates.Database
{
    public class DatesContext : DbContext
    {
        private static readonly DbContextOptions<DatesContext> DbContextOptions =
            new DbContextOptionsBuilder<DatesContext>()
                .UseSqlServer(
                    "Data Source=localhost,1433;" +
                    "Database=Dates;" +
                    "Trusted_Connection=True;" +
                    "Integrated Security=false;" +
                    "ConnectRetryCount=0;" +
                    "User Id=sa;" +
                    "Password=123456790abcDEF"
                )
                .Options;

        public DatesContext() : base(DbContextOptions)
        {
        }

        public DatesContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserAtGroup> UsersAtGroups { get; set; }
        public DbSet<EventAtGroup> EventsAtGroups { get; set; }
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