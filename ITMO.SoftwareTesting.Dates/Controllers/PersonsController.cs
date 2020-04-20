using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace ITMO.SoftwareTesting.Datings.Controllers
{
    [ApiController]
    [Route("api/persons")]
    public class PersonsController
    {
        private readonly IPersonsService personsService;

        public PersonsController(IPersonsService personsService)
        {
            this.personsService = personsService;
        }

        [HttpGet]
        [Route("search/{nicknameMask}")]
        public Task<List<PersonListItem>> SearchPeopleByNickname([FromRoute] string nicknameMask)
        {
            return personsService.SearchPeopleByNickname(nicknameMask);
        }
    }
}