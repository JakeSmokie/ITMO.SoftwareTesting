using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IGroupsService
    {
        Task<List<GroupDetails>> List();

        Task<int> UpsertGroup(GroupDetails details);
        Task DeleteGroup(int groupId);

        Task<List<PersonListItem>> Members(int groupId);

        Task InvitePerson(int groupId, int userId);
        Task DeletePerson(int groupId, int userId);

        Task<List<GroupDetails>> Invitations();
        Task AcceptInvitation(int groupId);
    }
}