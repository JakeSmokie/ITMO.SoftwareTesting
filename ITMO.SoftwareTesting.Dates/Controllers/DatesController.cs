using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITMO.SoftwareTesting.Datings.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/dates")]
    public class DatesController : ControllerBase
    {
        private readonly IDatesService datesService;

        public DatesController(IDatesService datesService)
        {
            this.datesService = datesService;
        }

        [HttpGet("{groupId?}")]
        public Task<List<Date>> List([FromRoute] int? groupId)
        {
            return datesService.List(groupId);
        }

        [HttpPost("create/{groupId}/{eventId}")]
        public Task Create([FromRoute] int groupId, [FromRoute] int eventId)
        {
            return datesService.Create(groupId, eventId);
        }
    }
}