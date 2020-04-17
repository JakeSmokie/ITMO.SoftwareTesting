using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IFavoriteEventsService
    {
        Task Add(int eventId);
        Task Remove(int eventId);
        Task<List<int>> List();
    }
}