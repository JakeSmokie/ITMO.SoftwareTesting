using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Exceptions;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Services.Services
{
    public class DatesService : IDatesService
    {
        private readonly IUserContext userContext;
        private readonly IDbContextFactory dbContextFactory;

        public DatesService(
            IUserContext userContext,
            IDbContextFactory dbContextFactory
        )
        {
            this.userContext = userContext;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task Create(int groupId, int eventId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var @group = await context.Groups
                .Where(x => x.Id == groupId)
                .Where(x => x.OwnerUserId == userContext.UserId)
                .Include(x => x.Events)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new DatesException("No such group was found");
            }

            group.Events.Add(new EventAtGroup {EventId = eventId});

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<Date>> List(int? groupId)
        {
            await using var context = dbContextFactory.Create();

            var dates = await context.Groups
                .Where(x => groupId == null || x.Id == groupId)
                .Where(x => x.Users.Select(x => x.UserId).Contains(userContext.UserId))
                .SelectMany(x => x.Events.Select(x => new Date {EventId = x.EventId, GroupId = x.GroupId}))
                .ToListAsync();

            return dates;
        }
    }
}