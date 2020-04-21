using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IDatesService
    {
        Task Create(int groupId, int eventId);
        Task<List<Date>> List(int? groupId);
    }
}