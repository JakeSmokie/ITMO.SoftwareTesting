using System.Linq;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Exceptions;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Dates.Tests.Extensions;
using ITMO.SoftwareTesting.Dates.Tests.Utils;
using Xunit;

namespace ITMO.SoftwareTesting.Dates.Tests.Tests
{
    public class GroupsTests
    {
        private readonly TestUserContext firstContext;
        private readonly IGroupsService firstGroups;
        private readonly IAuthenticationService firstAuth;

        private readonly TestUserContext secondContext;
        private readonly IGroupsService secondGroups;
        private readonly IAuthenticationService secondAuth;

        public GroupsTests()
        {
            var dbContextFactory = new InMemoryDbContextFactory();

            firstContext = new TestUserContext();
            firstGroups = new GroupsService(firstContext, dbContextFactory);
            firstAuth = new AuthenticationService(firstContext, dbContextFactory);

            secondContext = new TestUserContext();
            secondGroups = new GroupsService(secondContext, dbContextFactory);
            secondAuth = new AuthenticationService(secondContext, dbContextFactory);
        }

        [Fact]
        public async Task InsertionWorks()
        {
            const string name = "Dudes";
            const string purpose = "Friends parties";

            await firstContext.CreateUser(firstAuth);

            var groupId = await firstGroups.UpsertGroup(new GroupDetails {Name = name, Purpose = purpose});
            var group = (await firstGroups.List())[0];

            Assert.Equal(groupId, group.Id);
            Assert.Equal(name, group.Name);
            Assert.Equal(purpose, group.Purpose);
        }

        [Fact]
        public async Task UpdatingWorks()
        {
            const string name = "Dudes";
            const string purpose = "Friends parties";

            await firstContext.CreateUser(firstAuth);

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            await firstGroups.UpsertGroup(new GroupDetails {Id = groupId, Name = name, Purpose = purpose});

            var group = (await firstGroups.List())[0];

            Assert.Equal(groupId, group.Id);
            Assert.Equal(name, group.Name);
            Assert.Equal(purpose, group.Purpose);
        }

        [Fact]
        public async Task UpdatingFailsWhenUserDoesNotOwnAGroup()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await secondGroups.UpsertGroup(DefaultGroup(groupId));
            });

            Assert.Equal("You do not own this group. No permission for editing", exception.Message);
        }

        [Fact]
        public async Task UpdatingFailsWhenGroupDoesNotExist()
        {
            await firstContext.CreateUser(firstAuth);

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await firstGroups.UpsertGroup(DefaultGroup(100));
            });

            Assert.Equal("Group with such id does not exist", exception.Message);
        }

        [Fact]
        public async Task DeletionWorks()
        {
            await firstContext.CreateUser(firstAuth);

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var before = await firstGroups.List();

            await firstGroups.DeleteGroup(groupId);
            var after = await firstGroups.List();

            Assert.Single(before);
            Assert.Empty(after);
        }

        [Fact]
        public async Task DeletionFailsWhenUserDoesNotOwnAGroup()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await secondGroups.DeleteGroup(groupId);
            });

            Assert.Equal("You do not own this group. No permission for editing", exception.Message);
        }

        [Fact]
        public async Task DeletionFailsWhenGroupDoesNotExist()
        {
            await firstContext.CreateUser(firstAuth);
            var exception =
                await Assert.ThrowsAsync<DatesException>(async () => { await firstGroups.DeleteGroup(100); });

            Assert.Equal("Group with such id does not exist", exception.Message);
        }

        [Fact]
        public async Task MemberListingWorks()
        {
            await firstContext.CreateUser(firstAuth);

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var members = await firstGroups.Members(groupId);

            Assert.Single(members);
            Assert.Equal(firstContext.UserId, members[0].Id);
            Assert.False(members[0].Invited);
        }

        [Fact]
        public async Task MemberListingIsNotAllowedWhenUserIsNotInGroup()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await secondGroups.Members(groupId);
            });

            Assert.Equal("You do not belong to this group", exception.Message);
        }

        [Fact]
        public async Task InvitationWorks()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());

            await firstGroups.InvitePerson(groupId, secondContext.UserId);
            await firstGroups.InvitePerson(groupId, secondContext.UserId);
            await firstGroups.InvitePerson(groupId, secondContext.UserId);

            var members = await firstGroups.Members(groupId);
            var invitations = await secondGroups.Invitations();

            Assert.Single(invitations);
            Assert.Equal(groupId, invitations[0].Id);
            Assert.Equal(2, members.Count);
            Assert.Contains(secondContext.UserId, members.Where(x => x.Invited).Select(x => x.Id));
        }

        [Fact]
        public async Task InvitationFailsForNonOwner()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await secondGroups.InvitePerson(groupId, firstContext.UserId);
            });

            Assert.Equal("You are not an owner of this group", exception.Message);
        }

        [Fact]
        public async Task InvitationFailsForNonexistentGroup()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await firstGroups.InvitePerson(100, secondContext.UserId);
            });

            Assert.Equal("You are not an owner of this group", exception.Message);
        }

        [Fact]
        public async Task InvitationFailsForNonexistentUser()
        {
            await firstContext.CreateUser(firstAuth);
            var groupId = await firstGroups.UpsertGroup(DefaultGroup());

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await firstGroups.InvitePerson(groupId, 100);
            });

            Assert.Equal("Target user does not exist", exception.Message);
        }

        [Fact]
        public async Task InvitationIsSuppressedWhenUserInvitesHimself()
        {
            await firstContext.CreateUser(firstAuth);
            var groupId = await firstGroups.UpsertGroup(DefaultGroup());

            await firstGroups.InvitePerson(groupId, firstContext.UserId);
            var invitations = await firstGroups.Invitations();

            Assert.Empty(invitations);
        }

        [Fact]
        public async Task InvitationAcceptingWorks()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            await firstGroups.InvitePerson(groupId, secondContext.UserId);
            var before = await secondGroups.Invitations();

            await secondGroups.AcceptInvitation(groupId);

            var after = await secondGroups.Invitations();
            var members = await secondGroups.Members(groupId);
            var groups = await secondGroups.List();

            Assert.Single(before);
            Assert.Equal(groupId, before[0].Id);
            Assert.Empty(after);
            Assert.Contains(firstContext.UserId, members.Select(x => x.Id));
            Assert.Contains(secondContext.UserId, members.Select(x => x.Id));
            Assert.Single(groups);
            Assert.Equal(groupId, groups[0].Id);
        }

        [Fact]
        public async Task MemberDeletionWorks()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            await firstGroups.InvitePerson(groupId, secondContext.UserId);
            await secondGroups.AcceptInvitation(groupId);

            var before = await secondGroups.List();
            await firstGroups.DeletePerson(groupId, secondContext.UserId);

            var after = await secondGroups.List();

            Assert.Single(before);
            Assert.Equal(groupId, before[0].Id);
            Assert.Empty(after);
        }
        
        [Fact]
        public async Task MemberDeletionFailsForInvalidGroup()
        {
            await firstContext.CreateUser(firstAuth);
            await secondContext.CreateUser(secondAuth, "SecondFellow");

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await secondGroups.DeletePerson(groupId, firstContext.UserId);
            });

            Assert.Equal("You do not own this group", exception.Message);
        }

        [Fact]
        public async Task MemberDeletionIsSuppressedForSelfDeletion()
        {
            await firstContext.CreateUser(firstAuth);

            var groupId = await firstGroups.UpsertGroup(DefaultGroup());
            await firstGroups.DeletePerson(groupId, firstContext.UserId);

            var groups = await firstGroups.List();
            Assert.Single(groups);
        }

        [Fact]
        public async Task InvitationFailsForNonexistentInvitation()
        {
            await firstContext.CreateUser(firstAuth);

            var exception = await Assert.ThrowsAsync<DatesException>(async () =>
            {
                await firstGroups.AcceptInvitation(100);
            });

            Assert.Equal("No invitation found", exception.Message);
        }

        private static GroupDetails DefaultGroup(int id = 0)
        {
            return new GroupDetails {Name = "", Purpose = "", Id = id};
        }
    }
}