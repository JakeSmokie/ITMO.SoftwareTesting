using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Tests.Extensions;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class FavoriteEventsTests
    {
        private readonly TestUserContext context;
        private readonly IFavoriteEventsService events;
        private readonly IAuthenticationService auth;

        public FavoriteEventsTests()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            context = new TestUserContext();
            events = new FavoriteEventsService(context, dbContextFactory);
            auth = new AuthenticationService(context, dbContextFactory);
        }

        [Fact]
        public async Task EventsOperationsWork()
        {
            await context.CreateUser(auth);

            var first = await events.List();
            await events.Add(1);

            var second = await events.List();
            await events.Remove(1);
            await events.Remove(2);

            var third = await events.List();
            
            Assert.Empty(first);
            Assert.Equal(new List<int> {1}, second);
            Assert.Empty(third);
        }
    }
}