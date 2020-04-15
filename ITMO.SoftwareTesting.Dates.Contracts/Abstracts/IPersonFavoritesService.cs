using System.Collections.Generic;
using System.Threading.Tasks;
using ITMO.SoftwareTesting.Dates.Contracts.Models;

namespace ITMO.SoftwareTesting.Dates.Contracts.Abstracts
{
    public interface IPersonFavoritesService
    {
        Task AddInFavorites(string id);
        Task RemoveFromFavorites(string id);
        Task<List<PersonFavoritesListItem>> List();
    }
}