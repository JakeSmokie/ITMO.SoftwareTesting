using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Tests.Extensions;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class DatesTests
    {
        private readonly TestUserContext firstContext;
        private readonly IGroupsService firstGroups;
        private readonly IDatesService firstDates;
        private readonly IAuthenticationService firstAuth;

        private readonly TestUserContext secondContext;
        private readonly IGroupsService secondGroups;
        private readonly IDatesService secondDates;
        private readonly IAuthenticationService secondAuth;

        public DatesTests()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            firstContext = new TestUserContext();
            firstGroups = new GroupsService(firstContext, dbContextFactory);
            firstDates = new DatesService(firstContext, dbContextFactory);
            firstAuth = new AuthenticationService(firstContext, dbContextFactory);

            secondContext = new TestUserContext();
            secondGroups = new GroupsService(secondContext, dbContextFactory);
            secondDates = new DatesService(secondContext, dbContextFactory);
            secondAuth = new AuthenticationService(secondContext, dbContextFactory);
        }

        [Fact]
        public async Task DateCreationWorks()
        {
            const int eventId = 100;

            await firstContext.CreateUser(firstAuth);
            var groupId = await firstGroups.UpsertGroup(DefaultGroup());

            await firstDates.Create(groupId, eventId);
            var dates = await firstDates.List(groupId);

            Assert.Single(dates);
            Assert.Equal(groupId, dates[0].GroupId);
            Assert.Equal(eventId, dates[0].EventId);
        }

        [Fact]
        public async Task DatesListingWorksOnSpecificGroup()
        {
            await firstContext.CreateUser(firstAuth);
            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var secondGroupId = await firstGroups.UpsertGroup(DefaultGroup());

            await firstDates.Create(groupId, 100);
            await firstDates.Create(groupId, 101);
            await firstDates.Create(groupId, 102);
            await firstDates.Create(secondGroupId, 102);

            var dates = await firstDates.List(groupId);
            Assert.Equal(3, dates.Count);
        }

        [Fact]
        public async Task DatesListingWorksOnAllGroups()
        {
            await firstContext.CreateUser(firstAuth);
            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var secondGroupId = await firstGroups.UpsertGroup(DefaultGroup());

            await firstDates.Create(groupId, 100);
            await firstDates.Create(groupId, 101);
            await firstDates.Create(groupId, 102);
            await firstDates.Create(secondGroupId, 102);

            var dates = await firstDates.List(null);
            Assert.Equal(4, dates.Count);
        }

        [Fact]
        public async Task DatesListingIsSuppressedForNonMember()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "Second");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());

            await firstDates.Create(groupId, 100);
            await firstDates.Create(groupId, 101);
            await firstDates.Create(groupId, 102);

            var dates = await secondDates.List(groupId);
            Assert.Empty(dates);
        }

        private static Group DefaultGroup(int id = 0)
        {
            return new Group
            {
                Name = "", Purpose = "", Id = id
            };
        }
    }
}