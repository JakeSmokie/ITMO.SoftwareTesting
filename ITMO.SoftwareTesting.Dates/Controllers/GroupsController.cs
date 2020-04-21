using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITMO.SoftwareTesting.Datings.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupsService groupsService;

        public GroupsController(
            IGroupsService groupsService
        )
        {
            this.groupsService = groupsService;
        }

        [HttpPost]
        [Route("upsert")]
        public Task<int> UpsertGroup(Group details)
        {
            return groupsService.UpsertGroup(details);
        }

        [HttpGet]
        public Task<List<Group>> List()
        {
            return groupsService.List();
        }

        [HttpDelete]
        [Route("{id}")]
        public Task DeleteGroup([FromRoute] int id)
        {
            return groupsService.DeleteGroup(id);
        }

        [HttpGet]
        [Route("{id}/details")]
        public Task<GroupDetails> Details(int id)
        {
            return groupsService.Details(id);
        }

        [HttpPost]
        [Route("{group}/invite/{user}")]
        public Task InvitePerson([FromRoute] int group, [FromRoute] int user)
        {
            return groupsService.InvitePerson(group, user);
        }

        [HttpPost]
        [Route("{group}/delete/{user}")]
        public Task DeletePerson([FromRoute] int group, [FromRoute] int user)
        {
            return groupsService.DeletePerson(group, user);
        }

        [HttpGet]
        [Route("invitations")]
        public Task<List<Group>> Invitations()
        {
            return groupsService.Invitations();
        }

        [HttpPost]
        [Route("invitations/accept/{group}")]
        public Task AcceptInvitation(int group)
        {
            return groupsService.AcceptInvitation(group);
        }
    }
}