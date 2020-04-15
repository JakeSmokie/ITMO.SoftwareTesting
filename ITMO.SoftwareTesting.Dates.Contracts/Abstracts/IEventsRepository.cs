using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IEventsRepository
    {
        Task<List<IntegrationEvent>> Events();
        Task<List<IntegrationEventDetails>> EventDetails();
    }
}