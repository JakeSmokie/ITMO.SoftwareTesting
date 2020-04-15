using System;
using System.Threading;
using ITMO.SoftwareTesting.Dates.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Tests.Utils
{
    public class InMemoryDb : IDisposable
    {
        private static readonly ThreadLocal<Random> Random = new ThreadLocal<Random>(() => new Random());

        public readonly DatesContext Context;
        private readonly SqliteConnection connection;

        public InMemoryDb()
        {
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DatesContext>()
                .UseSqlite(connection)
                .Options;

            Context = new DatesContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            connection?.Dispose();
            Context?.Dispose();
        }
    }
}