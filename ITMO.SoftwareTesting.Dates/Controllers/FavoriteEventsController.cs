using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace ITMO.SoftwareTesting.Datings.Controllers
{
    [ApiController]
    [Route("/api/favorite-events")]
    public class FavoriteEventsController
    {
        private readonly IFavoriteEventsService favoriteEventsService;

        public FavoriteEventsController(IFavoriteEventsService favoriteEventsService)
        {
            this.favoriteEventsService = favoriteEventsService;
        }

        [HttpGet]
        public Task<List<int>> List()
        {
            return favoriteEventsService.List();
        }

        [HttpPost("add/{id}")]
        public Task Add([FromRoute] int id)
        {
            return favoriteEventsService.Add(id);
        }

        [HttpPost("remove/{id}")]
        public Task Remove([FromRoute] int id)
        {
            return favoriteEventsService.Remove(id);
        }
    }
}