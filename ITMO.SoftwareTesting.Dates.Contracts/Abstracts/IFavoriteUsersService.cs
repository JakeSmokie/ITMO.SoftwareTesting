using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IFavoriteUsersService
    {
        Task Add(int userId);
        Task Remove(int userId);
        Task<List<PersonFavorite>> List();
    }
}