using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Contracts.Constants;
using ITMO.SoftwareTesting.Dates.Contracts.Models;
using Newtonsoft.Json;

namespace ITMO.SoftwareTesting.Dates.Services.Services
{
    public class EventsRepository : IEventsRepository
    {
        private readonly HttpClient httpClient;

        public EventsRepository(
            IHttpClientFactory httpClientFactory
        )
        {
            httpClient = httpClientFactory.CreateClient(HttpClients.KudaGo);
        }

        public async Task<List<IntegrationEvent>> Events()
        {
            var responseMessage = await httpClient.GetAsync("https://kudago.com/public-api/v1.4/events/?location=spb");
            var responseBody = await responseMessage.Content.ReadAsStringAsync();

            var responseJson = JsonConvert.DeserializeAnonymousType(responseBody, new
            {
                results = new []
                {
                    new
                    {
                        id = 0,
                        title = ""
                    }
                }
            });

            return responseJson.results.Select(x => new IntegrationEvent
            {
                Id = x.id,
                Title = x.title
            }).ToList();
        }

        public Task<List<IntegrationEventDetails>> EventDetails()
        {
            throw new System.NotImplementedException();
        }
    }
}