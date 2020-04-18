using ITMO.SoftwareTesting.Dates.Database;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Datings.Database
{
    public class DbContextFactory : IDbContextFactory
    {
        public DatesContext Create()
        {
            return new DatesContext();
        }
    }
}