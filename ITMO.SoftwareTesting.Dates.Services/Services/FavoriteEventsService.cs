using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using ITMO.SoftwareTesting.Dates.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace ITMO.SoftwareTesting.Dates.Services.Services
{
    public class FavoriteEventsService : IFavoriteEventsService
    {
        private readonly IUserContext userContext;
        private readonly IDbContextFactory dbContextFactory;

        public FavoriteEventsService(
            IUserContext userContext,
            IDbContextFactory dbContextFactory
        )
        {
            this.userContext = userContext;
            this.dbContextFactory = dbContextFactory;
        }

        public async Task Add(int eventId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            context.FavoriteEvents.Add(new FavoriteEvent
            {
                UserId = userContext.UserId,
                EventId = eventId
            });

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task Remove(int eventId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var @event = await context.FavoriteEvents
                .Where(x => x.UserId == userContext.UserId)
                .Where(x => x.EventId == eventId)
                .FirstOrDefaultAsync();

            if (@event == null)
            {
                return;
            }

            context.Remove(@event);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<int>> List()
        {
            await using var context = dbContextFactory.Create();

            var events = await context.FavoriteEvents
                .Where(x => x.UserId == userContext.UserId)
                .Select(x => x.EventId)
                .ToListAsync();

            return events;
        }
    }
}