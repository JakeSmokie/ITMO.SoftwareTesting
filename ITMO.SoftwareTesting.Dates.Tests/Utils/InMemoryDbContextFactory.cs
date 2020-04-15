using ITMO.SoftwareTesting.Dates.Database;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Tests.Utils
{
    public class InMemoryDbContextFactory : IDbContextFactory
    {
        private readonly SqliteConnection connection;

        public InMemoryDbContextFactory()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
        }

        public DatesContext Create()
        {
            var options = new DbContextOptionsBuilder<DatesContext>()
                .UseSqlite(connection)
                .Options;

            var context = new DatesContext(options);
            context.Database.EnsureCreated();

            return context;
        }
    }
}