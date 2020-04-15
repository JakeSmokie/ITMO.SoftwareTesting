using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IGroupsService
    {
        Task<List<GroupDetails>> List();

        Task<GroupDetails> UpsertGroup(GroupDetails details);
        Task<GroupDetails> DeleteGroup(GroupDetails details);

        Task<List<PersonListItem>> Members(string groupId);

        Task InvitePerson(string nickname);
        Task DeletePerson(string nickname);

        Task<List<GroupDetails>> Invitations();
    }
}