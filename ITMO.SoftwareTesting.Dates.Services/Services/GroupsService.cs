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
    public class GroupsService : IGroupsService
    {
        private readonly IDbContextFactory dbContextFactory;
        private readonly IUserContext userContext;

        public GroupsService(IUserContext userContext, IDbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
            this.userContext = userContext;
        }

        public async Task<List<GroupDetails>> List()
        {
            await using var context = dbContextFactory.Create();

            var groups = await context.UsersAtGroups
                .Where(x => x.UserId == userContext.UserId)
                .Select(x => new GroupDetails
                {
                    Id = x.Group.Id,
                    Name = x.Group.Name,
                    Purpose = x.Group.Purpose,
                })
                .ToListAsync();

            return groups;
        }

        public async Task<int> UpsertGroup(GroupDetails details)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            if (details.Id == 0)
            {
                var newGroup = new Group
                {
                    Name = details.Name,
                    Purpose = details.Purpose,
                    OwnerUserId = userContext.UserId,
                    Users = new List<UserAtGroup> {new UserAtGroup {UserId = userContext.UserId}}
                };

                context.Groups.Add(newGroup);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return newGroup.Id;
            }

            var group = await context.Groups
                .Where(x => x.Id == details.Id)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new DatesException("Group with such id does not exist");
            }

            if (group.OwnerUserId != userContext.UserId)
            {
                throw new DatesException("You do not own this group. No permission for editing");
            }

            group.Name = details.Name;
            group.Purpose = details.Purpose;

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return details.Id;
        }

        public async Task DeleteGroup(int groupId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var group = await context.Groups
                .Where(x => x.Id == groupId)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new DatesException("Group with such id does not exist");
            }

            if (group.OwnerUserId != userContext.UserId)
            {
                throw new DatesException("You do not own this group. No permission for editing");
            }

            context.Groups.Remove(group);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<PersonListItem>> Members(int groupId)
        {
            await using var context = dbContextFactory.Create();

            var members = await context.UsersAtGroups
                .Where(x => x.GroupId == groupId)
                .Select(x => new PersonListItem
                {
                    Id = x.User.Id,
                    Nickname = x.User.Nickname,
                    Invited = false,
                })
                .ToListAsync();

            if (!members.Select(x => x.Id).Contains(userContext.UserId))
            {
                throw new DatesException("You do not belong to this group");
            }

            var invitations = await context.GroupInvitations
                .Where(x => x.GroupId == groupId)
                .Select(x => new PersonListItem
                {
                    Id = x.User.Id,
                    Nickname = x.User.Nickname,
                    Invited = true,
                })
                .ToListAsync();

            return members.Concat(invitations).ToList();
        }

        public async Task InvitePerson(int groupId, int userId)
        {
            if (userId == userContext.UserId)
            {
                return;
            }

            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var targetExists = await context.Users
                .Where(x => x.Id == userId)
                .AnyAsync();

            if (!targetExists)
            {
                throw new DatesException("Target user does not exist");
            }

            var isOwner = await context.Groups
                .Where(x => x.Id == groupId)
                .Select(x => x.OwnerUserId == userContext.UserId)
                .FirstOrDefaultAsync();

            if (!isOwner)
            {
                throw new DatesException("You are not an owner of this group");
            }

            var invitationExists = await context.GroupInvitations
                .Where(x => x.GroupId == groupId)
                .Where(x => x.UserId == userId)
                .AnyAsync();

            if (invitationExists)
            {
                return;
            }

            context.GroupInvitations
                .Add(new GroupInvitation
                {
                    GroupId = groupId,
                    UserId = userId
                });

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task DeletePerson(int groupId, int userId)
        {
            if (userId == userContext.UserId)
            {
                return;
            }

            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var group = await context.Groups
                .Where(x => x.Id == groupId)
                .Where(x => x.OwnerUserId == userContext.UserId)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                throw new DatesException("You do not own this group");
            }

            var userAtGroup = await context.UsersAtGroups
                .Where(x => x.GroupId == groupId)
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            context.Remove(userAtGroup);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<List<GroupDetails>> Invitations()
        {
            await using var context = dbContextFactory.Create();

            var groups = await context.GroupInvitations
                .Where(x => x.UserId == userContext.UserId)
                .Select(x => new GroupDetails
                {
                    Id = x.Group.Id,
                    Name = x.Group.Name,
                    Purpose = x.Group.Purpose
                })
                .ToListAsync();

            return groups;
        }

        public async Task AcceptInvitation(int groupId)
        {
            await using var context = dbContextFactory.Create();
            await using var transaction = await context.Database.BeginTransactionAsync();

            var invitation = await context.GroupInvitations
                .Where(x => x.GroupId == groupId)
                .Where(x => x.UserId == userContext.UserId)
                .FirstOrDefaultAsync();

            if (invitation == null)
            {
                throw new DatesException("No invitation found");
            }

            context.GroupInvitations.Remove(invitation);
            context.UsersAtGroups.Add(new UserAtGroup
            {
                GroupId = groupId,
                UserId = userContext.UserId
            });

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}