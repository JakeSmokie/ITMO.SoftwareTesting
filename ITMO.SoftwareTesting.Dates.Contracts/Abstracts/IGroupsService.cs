using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IGroupsService
    {
        Task<List<Group>> List();

        Task<int> UpsertGroup(Group details);
        Task DeleteGroup(int groupId);

        Task<GroupDetails> Details(int groupId);

        Task InvitePerson(int groupId, int userId);
        Task DeletePerson(int groupId, int userId);

        Task<List<Group>> Invitations();
        Task AcceptInvitation(int groupId);
    }
}